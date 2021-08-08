class MudElementReference {
constructor() {
this.listenerId = 0;
this.eventListeners = {};
}
focus (element) {
if (element)
{
element.focus();
}
}
focusFirst (element, skip = 0, min = 0) {
if (element)
{
let tabbables = getTabbableElements(element);
if (tabbables.length <= min)
element.focus();
else
tabbables[skip].focus();
}
}
focusLast (element, skip = 0, min = 0) {
if (element)
{
let tabbables = getTabbableElements(element);
if (tabbables.length <= min)
element.focus();
else
tabbables[tabbables.length - skip - 1].focus();
}
}
saveFocus (element) {
if (element)
{
element['mudblazor_savedFocus'] = document.activeElement;
}
}
restoreFocus (element) {
if (element)
{
let previous = element['mudblazor_savedFocus'];
delete element['mudblazor_savedFocus']
if (previous)
previous.focus();
}
}
selectRange(element, pos1, pos2) {
if (element)
{
if (element.createTextRange) {
let selRange = element.createTextRange();
selRange.collapse(true);
selRange.moveStart('character', pos1);
selRange.moveEnd('character', pos2);
selRange.select();
} else if (element.setSelectionRange) {
element.setSelectionRange(pos1, pos2);
} else if (element.selectionStart) {
element.selectionStart = pos1;
element.selectionEnd = pos2;
}
element.focus();
}
}
select(element) {
if (element)
{
element.select();
}
}
/**
* gets the client rect of the parent of the element
* @param {HTMLElement} element
*/
getClientRectFromParent(element) {
if (!element) return;
let parent = element.parentElement;
if (!parent) return;
return this.getBoundingClientRect(parent);
}
/**
* Gets the client rect of the first child of the element
* @param {any} element
*/
getClientRectFromFirstChild(element) {
if (!element) return;
let child = element.children && element.children[0];
if (!child) return;
return this.getBoundingClientRect(child);
}
getBoundingClientRect(element) {
if (!element) return;
var rect = JSON.parse(JSON.stringify(element.getBoundingClientRect()));
rect.scrollY = window.scrollY || document.documentElement.scrollTop;
rect.scrollX = window.scrollX || document.documentElement.scrollLeft;
rect.windowHeight = window.innerHeight;
rect.windowWidth = window.innerWidth;
return rect;
}
/**
* Returns true if the element has any ancestor with style position==="fixed"
* @param {Element} element
*/
hasFixedAncestors(element) {
for (; element && element !== document; element = element.parentNode) {
if (window.getComputedStyle(element).getPropertyValue("position") === "fixed")
return true;
}
return false
};
changeCss (element, css) {
if (element)
{
element.className = css;
}
}
changeCssVariable (element, name, newValue) {
if (element)
{
element.style.setProperty(name, newValue);
}
}
addEventListener (element, dotnet, event, callback, spec, stopPropagation) {
let listener = function (e) {
const args = Array.from(spec, x => serializeParameter(e, x));
dotnet.invokeMethodAsync(callback, ...args);
if (stopPropagation) {
e.stopPropagation();
}
};
element.addEventListener(event, listener);
this.eventListeners[++this.listenerId] = listener;
return this.listenerId;
}
removeEventListener (element, event, eventId) {
element.removeEventListener(event, this.eventListeners[eventId]);
delete this.eventListeners[eventId];
}
};
window.mudElementRef = new MudElementReference();
//Functions related to MudThrottledEventManager
class MudThrottledEventManager {
constructor() {
this.mapper = {};
}
subscribe(eventName, elementId, projection, throotleInterval, key, properties, dotnetReference) {
const handlerRef = this.throttleEventHandler.bind(this, key);
let elem = document.getElementById(elementId);
if (elem) {
elem.addEventListener(eventName, handlerRef, false);
let projector = null;
if (projection) {
const parts = projection.split('.');
let functionPointer = window;
let functionReferenceFound = true;
if (parts.length == 0 || parts.length == 1) {
functionPointer = functionPointer[projection];
}
else {
for (let i = 0; i < parts.length; i++) {
functionPointer = functionPointer[parts[i]];
if (!functionPointer) {
functionReferenceFound = false;
break;
}
}
}
if (functionReferenceFound === true) {
projector = functionPointer;
}
}
this.mapper[key] = {
eventName: eventName,
handler: handlerRef,
delay: throotleInterval,
timerId: -1,
reference: dotnetReference,
elementId: elementId,
properties: properties,
projection: projector,
};
}
}
throttleEventHandler(key, event) {
const entry = this.mapper[key];
if (!entry) {
return;
}
clearTimeout(entry.timerId);
entry.timerId = window.setTimeout(
this.eventHandler.bind(this, key, event),
entry.delay
);
}
eventHandler(key, event) {
const entry = this.mapper[key];
if (!entry) {
return;
}
var elem = document.getElementById(entry.elementId);
if (elem != event.srcElement) {
return;
}
const eventEntry = {};
for (var i = 0; i < entry.properties.length; i++) {
eventEntry[entry.properties[i]] = event[entry.properties[i]];
}
if (entry.projection) {
if (typeof entry.projection === "function") {
entry.projection.apply(null, [eventEntry, event]);
}
}
entry.reference.invokeMethodAsync('OnEventOccur', key, JSON.stringify(eventEntry));
}
unsubscribe(key) {
const entry = this.mapper[key];
if (!entry) {
return;
}
entry.reference = null;
const elem = document.getElementById(entry.elementId);
if (elem) {
elem.removeEventListener(entry.eventName, entry.handler, false);
}
delete this.mapper[key];
}
};
window.mudThrottledEventManager = new MudThrottledEventManager();
window.mudEventProjections = {
correctOffset: function (eventEntry, event) {
var target = event.target.getBoundingClientRect();
eventEntry.offsetX = event.clientX - target.x;
eventEntry.offsetY = event.clientY - target.y;
}
};
window.getTabbableElements = (element) => {
return element.querySelectorAll(
"a[href]:not([tabindex='-1'])," +
"area[href]:not([tabindex='-1'])," +
"button:not([disabled]):not([tabindex='-1'])," +
"input:not([disabled]):not([tabindex='-1']):not([type='hidden'])," +
"select:not([disabled]):not([tabindex='-1'])," +
"textarea:not([disabled]):not([tabindex='-1'])," +
"iframe:not([tabindex='-1'])," +
"details:not([tabindex='-1'])," +
"[tabindex]:not([tabindex='-1'])," +
"[contentEditable=true]:not([tabindex='-1']"
);
};
//from: https://github.com/RemiBou/BrowserInterop
window.serializeParameter = (data, spec) => {
if (typeof data == "undefined" ||
data === null) {
return null;
}
if (typeof data === "number" ||
typeof data === "string" ||
typeof data == "boolean") {
return data;
}
let res = (Array.isArray(data)) ? [] : {};
if (!spec) {
spec = "*";
}
for (let i in data) {
let currentMember = data[i];
if (typeof currentMember === 'function' || currentMember === null) {
continue;
}
let currentMemberSpec;
if (spec != "*") {
currentMemberSpec = Array.isArray(data) ? spec : spec[i];
if (!currentMemberSpec) {
continue;
}
} else {
currentMemberSpec = "*"
}
if (typeof currentMember === 'object') {
if (Array.isArray(currentMember) || currentMember.length) {
res[i] = [];
for (let j = 0; j < currentMember.length; j++) {
const arrayItem = currentMember[j];
if (typeof arrayItem === 'object') {
res[i].push(this.serializeParameter(arrayItem, currentMemberSpec));
} else {
res[i].push(arrayItem);
}
}
} else {
//the browser provides some member (like plugins) as hash with index as key, if length == 0 we shall not convert it
if (currentMember.length === 0) {
res[i] = [];
} else {
res[i] = this.serializeParameter(currentMember, currentMemberSpec);
}
}
} else {
// string, number or boolean
if (currentMember === Infinity) { //inifity is not serialized by JSON.stringify
currentMember = "Infinity";
}
if (currentMember !== null) { //needed because the default json serializer in jsinterop serialize null values
res[i] = currentMember;
}
}
}
return res;
};
/**
*
* @param {{id:string, isVisible:boolean, cssPosition:string}} portalModel
* @param {RectInfo} portalAnchor
* @returns
*/
function mudHandlePortal(portalModel, portalAnchor) {
let portalledElement = document.getElementById(portalModel.id);
if (!portalledElement || !portalAnchor) return;
let action = portalModel.isVisible ? 'add' : 'remove';
portalledElement.firstElementChild.classList[action](
'mud-popover-open',
'mud-tooltip-visible'
);
if (!portalModel.isVisible) return;
let anchorRect = window.mudElementRef.getBoundingClientRect(
portalAnchor.parentElement
);
let fragmentRect = window.mudElementRef.getBoundingClientRect(
portalAnchor.firstElementChild
);
let correctedAnchorRect = mudCorrectAnchorBoundaries(anchorRect, fragmentRect);
let style = mudGetAnchorStyle(correctedAnchorRect, portalModel);
portalledElement.style.cssText = style;
}
function mudGetAnchorStyle(anchorRect, portalModel) {
let top = 0;
if (portalModel.cssPosition == 'fixed' && anchorRect != null) top = anchorRect.top;
else if (anchorRect != null) top = anchorRect.absoluteTop;
let left = 0;
if (portalModel.cssPosition == 'fixed' && anchorRect != null) left = anchorRect.left
else if (anchorRect != null) left =  anchorRect.absoluteLeft;
let height = 0;
let width = 0;
if (anchorRect != null) {
    height = anchorRect.height;
    width = anchorRect.width;
}
let position = !portalModel.isVisible ? 'fixed' : portalModel.cssPosition;
let zIndex = portalModel.cssPosition == 'fixed' && 1400;
return `top:${top}px;left:${left}px;height:${height}px;width:${width}px;position:${position};z-index:${zIndex}`;
}
class RectInfo {
/**
*
* @param {RectInfo} rect
*/
constructor(rect) {
this.left = rect.left;
this.top = rect.top;
this.width = rect.width;
this.height = rect.height;
this.windowHeight = rect.windowHeight;
this.windowWidth = rect.windowWidth;
this.scrollX = rect.scrollX;
this.scrollY = rect.scrollY;
}
get right() {
return this.left + this.width;
}
get bottom() {
return this.top + this.height;
}
get absoluteLeft() {
return this.left + this.scrollX;
}
get absoluteTop() {
return this.top + this.scrollY;
}
get absoluteRight() {
return this.right + this.scrollX;
}
get absoluteBottom() {
return this.bottom + this.scrollY;
}
//check if the rect is outside of the viewport
get isOutsideBottom() {
return this.bottom > this.windowHeight;
}
get isOutsideLeft() {
return this.left < 0;
}
get isOutsideTop() {
return this.top < 0;
}
get isOutsideRight() {
return this.right > this.windowWidth;
}
}
/**
*
* @param {RectInfo} anchorRect
* @param {RectInfo} fragmentRect
* @returns
*/
function mudCorrectAnchorBoundaries(anchorRect, fragmentRect) {
if (!fragmentRect || !anchorRect) return;
anchorRect = new RectInfo(anchorRect);
fragmentRect = new RectInfo(fragmentRect);
let rectified = mudShallowClone(anchorRect);
let fragmentIsAboveorBelowAnchor =
fragmentRect.top > anchorRect.bottom ||
fragmentRect.bottom < anchorRect.top;
// comes out at the bottom
if (fragmentRect.isOutsideBottom) {
rectified.top -=
2 * (fragmentRect.top - anchorRect.bottom) +
anchorRect.height +
fragmentRect.height;
}
// comes out at the top
if (fragmentRect.isOutsideTop) {
rectified.top +=
2 * Math.abs(anchorRect.top - fragmentRect.bottom) +
anchorRect.height +
fragmentRect.height;
}
// comes out at the left
if (fragmentRect.isOutsideLeft) {
rectified.left += fragmentIsAboveorBelowAnchor
? anchorRect.left - fragmentRect.left
: 2 * Math.abs(anchorRect.left - fragmentRect.right) +
fragmentRect.width +
anchorRect.width;
}
// comes out at the right
if (fragmentRect.isOutsideRight) {
rectified.left -= fragmentIsAboveorBelowAnchor
? fragmentRect.right - anchorRect.right
: 2 * Math.abs(fragmentRect.left - anchorRect.right) +
fragmentRect.width +
anchorRect.width;
}
return rectified;
}
function mudShallowClone(obj) {
return Object.create(
Object.getPrototypeOf(obj),
Object.getOwnPropertyDescriptors(obj)
);
}
class MudResizeListener {
constructor() {
this.logger = function (message) { };
this.options = {};
this.throttleResizeHandlerId = -1;
this.dotnet = undefined;
this.breakpoint = -1;
}
listenForResize (dotnetRef, options) {
if (this.dotnet) {
this.options = options;
return;
}
//this.logger("[MudBlazor] listenForResize:", { options, dotnetRef });
this.options = options;
this.dotnet = dotnetRef;
this.logger = options.enableLogging ? console.log : (message) => { };
this.logger(`[MudBlazor] Reporting resize events at rate of: ${(this.options || {}).reportRate || 100}ms`);
window.addEventListener("resize", this.throttleResizeHandler.bind(this), false);
if (!this.options.suppressInitEvent) {
this.resizeHandler();
}
this.breakpoint = this.getBreakpoint(window.innerWidth);
}
throttleResizeHandler() {
clearTimeout(this.throttleResizeHandlerId);
//console.log("[MudBlazor] throttleResizeHandler ", {options:this.options});
this.throttleResizeHandlerId = window.setTimeout(this.resizeHandler.bind(this), ((this.options || {}).reportRate || 100));
}
resizeHandler() {
if (this.options.notifyOnBreakpointOnly) {
let bp = this.getBreakpoint(window.innerWidth);
if (bp == this.breakpoint) {
return;
}
this.breakpoint = bp;
}
try {
//console.log("[MudBlazor] RaiseOnResized invoked");
this.dotnet.invokeMethodAsync('RaiseOnResized',
{
height: window.innerHeight,
width: window.innerWidth
}, this.getBreakpoint(window.innerWidth));
//this.logger("[MudBlazor] RaiseOnResized invoked");
} catch (error) {
this.logger("[MudBlazor] Error in resizeHandler:", { error });
}
}
cancelListener () {
this.dotnet = undefined;
//console.log("[MudBlazor] cancelListener");
window.removeEventListener("resize", this.throttleResizeHandler);
}
matchMedia (query) {
let m = window.matchMedia(query).matches;
//this.logger(`[MudBlazor] matchMedia "${query}": ${m}`);
return m;
}
getBrowserWindowSize () {
//this.logger("[MudBlazor] getBrowserWindowSize");
return {
height: window.innerHeight,
width: window.innerWidth
};
}
getBreakpoint (width) {
if (width >= this.options.breakpointDefinitions["Xl"])
return 4;
else if (width >= this.options.breakpointDefinitions["Lg"])
return 3;
else if (width >= this.options.breakpointDefinitions["Md"])
return 2;
else if (width >= this.options.breakpointDefinitions["Sm"])
return 1;
else //Xs
return 0;
}
};
window.mudResizeListener = new MudResizeListener();
class MudResizeObserverFactory {
constructor() {
this._maps = {};
}
connect(id, dotNetRef, elements, elementIds, options) {
var existingEntry = this._maps[id];
if (!existingEntry) {
var observer = new MudResizeObserver(dotNetRef, options);
this._maps[id] = observer;
}
var result = this._maps[id].connect(elements, elementIds);
return result;
}
disconnect(id, element) {
//I can't think about a case, where this can be called, without observe has been called before
//however, a check is not harmful either
var existingEntry = this._maps[id];
if (existingEntry) {
existingEntry.disconnect(element);
}
}
cancelListener(id) {
//cancelListener is called during dispose of .net instance
//in rare cases it could be possible, that no object has been connect so far
//and no entry exists. Therefore, a little check to prevent an error in this case
var existingEntry = this._maps[id];
if (existingEntry) {
existingEntry.cancelListener();
delete this._maps[id];
}
}
}
class MudResizeObserver {
constructor(dotNetRef, options) {
this.logger = options.enableLogging ? console.log : (message) => { };
this.options = options;
this._dotNetRef = dotNetRef
var delay = (this.options || {}).reportRate || 200;
this.throttleResizeHandlerId = -1;
var observervedElements = [];
this._observervedElements = observervedElements;
this.logger('[MudBlazor | ResizeObserver] Observer initilized');
this._resizeObserver = new ResizeObserver(entries => {
var changes = [];
this.logger('[MudBlazor | ResizeObserver] changes detected');
for (let entry of entries) {
var target = entry.target;
var affectedObservedElement = observervedElements.find((x) => x.element == target);
if (affectedObservedElement) {
var size = entry.target.getBoundingClientRect();
if (affectedObservedElement.isInitilized == true) {
changes.push({ id: affectedObservedElement.id, size: size });
}
else {
affectedObservedElement.isInitilized = true;
}
}
}
if (changes.length > 0) {
if (this.throttleResizeHandlerId >= 0) {
clearTimeout(this.throttleResizeHandlerId);
}
this.throttleResizeHandlerId = window.setTimeout(this.resizeHandler.bind(this, changes), delay);
}
});
}
resizeHandler(changes) {
try {
this.logger("[MudBlazor | ResizeObserver] OnSizeChanged handler invoked");
this._dotNetRef.invokeMethodAsync("OnSizeChanged", changes);
} catch (error) {
this.logger("[MudBlazor | ResizeObserver] Error in OnSizeChanged handler:", { error });
}
}
connect(elements, ids) {
var result = [];
this.logger('[MudBlazor | ResizeObserver] Start observing elements...');
for (var i = 0; i < elements.length; i++) {
var newEntry = {
element: elements[i],
id: ids[i],
isInitilized: false,
};
this.logger("[MudBlazor | ResizeObserver] Start observing element:", { newEntry });
result.push(elements[i].getBoundingClientRect());
this._observervedElements.push(newEntry);
this._resizeObserver.observe(elements[i]);
}
return result;
}
disconnect(elementId) {
this.logger('[MudBlazor | ResizeObserver] Try to unobserve element with id', { elementId });
var affectedObservedElement = this._observervedElements.find((x) => x.id == elementId);
if (affectedObservedElement) {
var element = affectedObservedElement.element;
this._resizeObserver.unobserve(element);
this.logger('[MudBlazor | ResizeObserver] Element found. Ubobserving size changes of element', { element });
var index = this._observervedElements.indexOf(affectedObservedElement);
this._observervedElements.splice(index, 1);
}
}
cancelListener() {
this.logger('[MudBlazor | ResizeObserver] Closing ResizeObserver. Detaching all observed elements');
this._resizeObserver.disconnect();
this._dotNetRef = undefined;
}
}
window.mudResizeObserver = new MudResizeObserverFactory();
//Functions related to scroll events
class MudScrollListener {
constructor() {
this.throttleScrollHandlerId = -1;
}
// subscribe to throttled scroll event
listenForScroll(dotnetReference, selector) {
//if selector is null, attach to document
let element = selector
? document.querySelector(selector)
: document;
// add the event listener
element.addEventListener(
'scroll',
this.throttleScrollHandler.bind(this, dotnetReference),
false
);
}
// fire the event just once each 100 ms, **it's hardcoded**
throttleScrollHandler(dotnetReference, event) {
clearTimeout(this.throttleScrollHandlerId);
this.throttleScrollHandlerId = window.setTimeout(
this.scrollHandler.bind(this, dotnetReference, event),
100
);
}
// when scroll event is fired, pass this information to
// the RaiseOnScroll C# method of the ScrollListener
// We pass the scroll coordinates of the element and
// the boundingClientRect of the first child, because
// scrollTop of body is always 0. With this information,
// we can trigger C# events on different scroll situations
scrollHandler(dotnetReference, event) {
try {
let element = event.target;
//data to pass
let scrollTop = element.scrollTop;
let scrollHeight = element.scrollHeight;
let scrollWidth = element.scrollWidth;
let scrollLeft = element.scrollLeft;
let nodeName = element.nodeName;
//data to pass
let firstChild = element.firstElementChild;
let firstChildBoundingClientRect = firstChild.getBoundingClientRect();
//invoke C# method
dotnetReference.invokeMethodAsync('RaiseOnScroll', {
firstChildBoundingClientRect,
scrollLeft,
scrollTop,
scrollHeight,
scrollWidth,
nodeName,
});
} catch (error) {
console.log('[MudBlazor] Error in scrollHandler:', { error });
}
}
//remove event listener
cancelListener(selector) {
let element = selector
? document.querySelector(selector)
: document.documentElement;
element.removeEventListener('scroll', this.throttleScrollHandler);
}
};
window.mudScrollListener = new MudScrollListener();
class MudScrollManager {
//scrolls to an Id. Useful for navigation to fragments
scrollToFragment (elementId, behavior) {
let element = document.getElementById(elementId);
if (element) {
element.scrollIntoView({ behavior, block: 'center', inline: 'start' });
}
}
//scrolls to year in MudDatePicker
scrollToYear (elementId, offset) {
let element = document.getElementById(elementId);
if (element) {
element.parentNode.scrollTop = element.offsetTop - element.parentNode.offsetTop - element.scrollHeight * 3;
}
}
// scrolls down or up in a select input
//increment is 1 if moving dow and -1 if moving up
//onEdges is a boolean. If true, it waits to reach the bottom or the top
//of the container to scroll.
scrollToListItem (elementId, increment, onEdges) {
let element = document.getElementById(elementId);
if (element) {
//this is the scroll container
let parent = element.parentElement;
//reset the scroll position when close the menu
if (increment == 0) {
parent.scrollTop = 0;
return;
}
//position of the elements relative to the screen, so we can compare
//one with the other
//e:element; p:parent of the element; For example:eBottom is the element bottom
let { bottom: eBottom, height: eHeight, top: eTop } = element.getBoundingClientRect();
let { bottom: pBottom, top: pTop } = parent.getBoundingClientRect();
if (
//if element reached bottom and direction is down
((pBottom - eBottom <= 0) && increment > 0)
//or element reached top and direction is up
|| ((eTop - pTop <= 0) && increment < 0)
// or scroll is not constrained to the Edges
|| !onEdges
) {
parent.scrollTop += eHeight * increment;
}
}
}
//scrolls to the selected element. Default is documentElement (i.e., html element)
scrollTo (selector, left, top, behavior) {
let element = document.querySelector(selector) || document.documentElement;
element.scrollTo({ left, top, behavior });
}
//locks the scroll of the selected element. Default is body
lockScroll (selector, lockclass) {
let element = document.querySelector(selector) || document.body;
//if the body doesn't have a scroll bar, don't add the lock class
let hasScrollBar = window.innerWidth > document.body.clientWidth;
if (hasScrollBar) {
element.classList.add(lockclass);
}
}
//unlocks the scroll. Default is body
unlockScroll (selector, lockclass) {
let element = document.querySelector(selector) || document.body;
element.classList.remove(lockclass);
}
};
window.mudScrollManager = new MudScrollManager();
//Functions related to the scroll spy
class MudScrollSpy {
constructor() {
this.scrollToSectionRequested = null;
this.lastKnowElement = null;
//needed as variable to remove the event listeners
this.handlerRef = null;
}
// subscribe to relevant events
spying(dotnetReference, selector) {
this.scrollToSectionRequested = null;
this.lastKnowElement = null;
this.handlerRef = this.handleScroll.bind(this, selector, dotnetReference);
// add the event for scroll. In case of zooming this event is also fired
document.addEventListener('scroll', this.handlerRef, true);
// a window resize could change the size of the relevant viewport
window.addEventListener('resize', this.handlerRef, true);
}
// handle the document scroll event and if needed, fires the .NET event
handleScroll(dotnetReference, selector, event) {
const elements = document.getElementsByClassName(selector);
if (elements.length === 0) {
return;
}
const center = window.innerHeight / 2.0;
let minDifference = Number.MAX_SAFE_INTEGER;
let elementId = '';
for (let i = 0; i < elements.length; i++) {
const element = elements[i];
const rect = element.getBoundingClientRect();
const diff = Math.abs(rect.top - center);
if (diff < minDifference) {
minDifference = diff;
elementId = element.id;
}
}
if (document.getElementById(elementId).getBoundingClientRect().top < window.innerHeight * 0.8 === false) {
return;
}
if (this.scrollToSectionRequested != null) {
if (this.scrollToSectionRequested == ' ' && window.scrollY == 0) {
this.scrollToSectionRequested = null;
}
else {
if (elementId === this.scrollToSectionRequested) {
this.scrollToSectionRequested = null;
}
}
return;
}
if (elementId != this.lastKnowElement) {
this.lastKnowElement = elementId;
history.replaceState(null, '', window.location.pathname + "#" + elementId);
dotnetReference.invokeMethodAsync('SectionChangeOccured', elementId);
}
}
activateSection(sectionId) {
const element = document.getElementById(sectionId);
if (element) {
this.lastKnowElement = sectionId;
history.replaceState(null, '', window.location.pathname + "#" + sectionId);
}
}
scrollToSection(sectionId) {
if (sectionId) {
let element = document.getElementById(sectionId);
if (element) {
this.scrollToSectionRequested = sectionId;
element.scrollIntoView({ behavior: 'smooth', block: 'center', inline: 'start' });
}
}
else {
window.scrollTo({ top: 0, behavior: 'smooth' });
this.scrollToSectionRequested = ' ';
}
}
//remove event listeners
unspy() {
document.removeEventListener('scroll', this.handlerRef, true);
window.removeEventListener('resize', this.handlerRef, true);
}
};
window.mudScrollSpy = new MudScrollSpy();
class MudWindow {
copyToClipboard (text) {
navigator.clipboard.writeText(text);
}
changeCssById (id, css) {
var element = document.getElementById(id);
if (element) {
element.className = css;
}
}
changeGlobalCssVariable (name, newValue) {
document.documentElement.style.setProperty(name, newValue);
}
// Needed as per https://stackoverflow.com/questions/62769031/how-can-i-open-a-new-window-without-using-js
open (args) {
window.open(args);
}
};
window.mudWindow = new MudWindow();
