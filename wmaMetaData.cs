using System;
using System.Collections.Generic;
using System.Text;
using WMFSDKWrapper;

/*        
 Based on: "Accessing WMF metadata with C#" By Kris Rudin http://www.codeproject.com/KB/audio-video/MetaDataReader.aspx
 */


namespace MarkAble2
{

    /// This class contains the functionality

    /// for handling interaction with the media file

    /// metadata, via the WMF SDK managed wrapper class.

    public class wmaMetaData
    {

        /// Default constructor

        public wmaMetaData()
        {
        }

        /// Method to obtain a metadata attribute by passing in its name. 
        /// Assumes the metadata type is STRING.
        /// Uses the SDK function GetAttributeByName.
        ///
        /// param name="filename" - the filename
        ///            (including path) of media file to interrogate
        /// param name="attrName" - the name of the field we're looking for
        /// returns - the value of the named attribute,
        ///           empty string if not found, or error message

        public bool GetFieldByName(string fileName, string attrName, out string attrValue)
        {
            try
            {
                //object used to access WMF file 

                IWMMetadataEditor MetadataEditor;
                //object to use access metadata 

                IWMHeaderInfo3 HeaderInfo3;
                //media stream to interrogate
 
                //DEBUG
                attrName = "Is_Protected";
                //END DEBUG


                WMFSDKFunctions.WMCreateEditor(out MetadataEditor);

                MetadataEditor.Open(fileName);

                HeaderInfo3 = (IWMHeaderInfo3)MetadataEditor;


                attrValue = GetAttrValue(HeaderInfo3, attrName);

                MetadataEditor.Close();
                
                return (! String.IsNullOrEmpty(attrValue));
            }
            catch (Exception e)
            {
                attrValue = "ERROR: " + e.Message;
                return false;
            }
        }

        private string GetAttrValue(IWMHeaderInfo3 HeaderInfo3, string attrName)
        {
            ushort streamNum = 0;
            //data type of attribute

            WMT_ATTR_DATATYPE wAttribType;
            //value of attribute (as returned by method call)

            byte[] pbAttribValue = null;
            //length of attribute (byte array)

            ushort wAttribValueLen = 0;

            try
            {
                //make call to get attribute length
                HeaderInfo3.GetAttributeByName(ref streamNum, attrName,
                                               out wAttribType, pbAttribValue, ref wAttribValueLen);

                //set byte array length
                pbAttribValue = new byte[wAttribValueLen];

                //make call again, which will get value 
                //into correct-length byte array

                HeaderInfo3.GetAttributeByName(ref streamNum,
                                               attrName, out wAttribType,
                                               pbAttribValue,
                                               ref wAttribValueLen);

                switch(wAttribType)
                {
                    case WMT_ATTR_DATATYPE.WMT_TYPE_DWORD:
                        return null;
                    case WMT_ATTR_DATATYPE.WMT_TYPE_STRING:
                        return ConvertAttrToString(pbAttribValue, wAttribValueLen);
                    case WMT_ATTR_DATATYPE.WMT_TYPE_BINARY:
                        return null;
                    case WMT_ATTR_DATATYPE.WMT_TYPE_BOOL:
                        if (wAttribValueLen > 0)
                        {
                            return pbAttribValue[0] == 0 ? "false" : "true";
                        }
                        return "false";
                    case WMT_ATTR_DATATYPE.WMT_TYPE_QWORD:
                        return null;
                    case WMT_ATTR_DATATYPE.WMT_TYPE_WORD:
                        return null;
                    case WMT_ATTR_DATATYPE.WMT_TYPE_GUID:
                        return ConvertAttrToString(pbAttribValue, wAttribValueLen); ;
                    default:
                        return null;
                }
            }
            catch //(Exception ex)
            {
                return null; //ex.Message;
            }
        }

//end method


        /// Method to convert byte array value into string. 
        /// (From the Microsoft WMF SDK sample.)
        ///
        /// param name="pbValue" - byte array value of attribute
        /// param name="dwValueLen" - Length of byte array

        private string ConvertAttrToString(byte[] pbValue, ushort dwValueLen)
        {
            string Value = "";

            if (0 == dwValueLen)
            {
                Value = "";
            }
            else
            {
                if ((0xFE == Convert.ToInt16(pbValue[0])) &&
                     (0xFF == Convert.ToInt16(pbValue[1])))
                {
                    Value = "UTF-16LE BOM+";

                    if (4 <= dwValueLen)
                    {
                        for (int i = 0; i < pbValue.Length - 2; i += 2)
                        {
                            Value +=
                              Convert.ToString(BitConverter.ToChar(pbValue, i));
                        }
                    }
                }
                else if ((0xFF == Convert.ToInt16(pbValue[0])) &&
                          (0xFE == Convert.ToInt16(pbValue[1])))
                {
                    Value = "UTF-16BE BOM+";
                    if (4 <= dwValueLen)
                    {
                        for (int i = 0; i < pbValue.Length - 2; i += 2)
                        {
                            Value +=
                              Convert.ToString(BitConverter.ToChar(pbValue, i));
                        }
                    }
                }
                else
                {
                    Value = "";
                    if (2 <= dwValueLen)
                    {
                        for (int i = 0; i < pbValue.Length - 2; i += 2)
                        {
                            Value +=
                              Convert.ToString(BitConverter.ToChar(pbValue, i));
                        }
                    }
                }
            }//end else not a 0-length string


            return Value;

        }//end method


    }//end class

}
