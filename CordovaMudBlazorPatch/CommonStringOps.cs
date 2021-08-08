/******************************************************************************************************************************/
/*** AUTHOR NAME : JOHN KENEDY                                                                                              ***/
/*** EMAIL       : sorainnosia@gmail.com                                                                                    ***/
/*** History     : 18 Dec 2016 - JOHN KENEDY - Initial Development                                                          ***/
/***             :                                                                                                          ***/
/******************************************************************************************************************************/
//-----------------------------------------------------------------------------------------------------------------------------
// Author    : John Kenedy
// Email     : jokenjp@yahoo.com
// WebSite   : http://www.scrappermin.com
// 
// You are feel free to use this source code as you feel like but you need to retain the original ownership information which
// this source code is written originally by John Kenedy (jokenjp@yahoo.com) on every copy you distribute whether you have
// modified or not. You can add your contribution such as patches, new features, and adding your name as contributor without
// deleting or modifying original author.
//
// This source code is provided "AS IS" without warranty, the author shall not be held liable for any damage caused by this
// software whether material or immaterial.
//-----------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;

namespace Common
{
    /// <summary>
    /// String operations class. This class provide methods to operate on string using Method Extension feature.
    /// </summary>
    public static class CommonStringOps
    {
        /// <summary><![CDATA[
        /// Search for tag starting from in front
        /// Example, startTag='<div>', endTag='</div>'
        /// Data :
        /// <div>http://data1 Data 1</div><div id='unique'>more</div>
        /// <div>http://data2 Data 2</div><div id='unique'>more</div>
        /// ...
        /// Return : 'http://data1 Data 1', 'http://data2 Data 2', ... in IList ]]>
        /// </summary>
        /// <param name="webContent">The data that is being parsed</param>
        /// <param name="tagStart">The start of the data</param>
        /// <param name="tagEnd">The end of the data</param>
        /// <returns>Return all the tag matches</returns>
        public static string[] TagMatch(this string webContent, string tagStart, string tagEnd)
        {
            List<string> rs = new List<string>();
            string search = tagStart;
            int start = webContent.IndexOf(search);
            int end = 0;
            while (start != -1)
            {
                if (start == -1) break;
                if (start + search.Length >= webContent.Length) break;
                end = webContent.IndexOf(tagEnd, start + search.Length, StringComparison.CurrentCultureIgnoreCase);
                if (end == -1) break;
                
                string temp = webContent.Substring(start + search.Length, end - start - search.Length);
                rs.Add(temp);

                start = webContent.IndexOf(search, end + tagEnd.Length, StringComparison.CurrentCultureIgnoreCase);
            }
            return rs.ToArray();
        }

    }
}
