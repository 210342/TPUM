/* ========================================================================
 * Copyright (c) 2005-2016 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace TPUM
{
    #region DataType Identifiers
    /// <summary>
    /// A class that declares constants for all DataTypes in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypes
    {
        /// <summary>
        /// The identifier for the Book DataType.
        /// </summary>
        public const uint Book = 1;

        /// <summary>
        /// The identifier for the Author DataType.
        /// </summary>
        public const uint Author = 4;
    }
    #endregion

    #region Variable Identifiers
    /// <summary>
    /// A class that declares constants for all Variables in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <summary>
        /// The identifier for the Book_Title Variable.
        /// </summary>
        public const uint Book_Title = 2;

        /// <summary>
        /// The identifier for the Book_Id Variable.
        /// </summary>
        public const uint Book_Id = 20;

        /// <summary>
        /// The identifier for the Book_Authors Variable.
        /// </summary>
        public const uint Book_Authors = 25;

        /// <summary>
        /// The identifier for the Author_Books Variable.
        /// </summary>
        public const uint Author_Books = 19;

        /// <summary>
        /// The identifier for the Author_Id Variable.
        /// </summary>
        public const uint Author_Id = 21;

        /// <summary>
        /// The identifier for the Author_FirstName Variable.
        /// </summary>
        public const uint Author_FirstName = 22;

        /// <summary>
        /// The identifier for the Author_LastName Variable.
        /// </summary>
        public const uint Author_LastName = 23;

        /// <summary>
        /// The identifier for the Author_NickName Variable.
        /// </summary>
        public const uint Author_NickName = 24;

        /// <summary>
        /// The identifier for the TPUM_XmlSchema Variable.
        /// </summary>
        public const uint TPUM_XmlSchema = 10;

        /// <summary>
        /// The identifier for the TPUM_XmlSchema_NamespaceUri Variable.
        /// </summary>
        public const uint TPUM_XmlSchema_NamespaceUri = 12;

        /// <summary>
        /// The identifier for the TPUM_BinarySchema Variable.
        /// </summary>
        public const uint TPUM_BinarySchema = 13;

        /// <summary>
        /// The identifier for the TPUM_BinarySchema_NamespaceUri Variable.
        /// </summary>
        public const uint TPUM_BinarySchema_NamespaceUri = 15;
    }
    #endregion

    #region DataType Node Identifiers
    /// <summary>
    /// A class that declares constants for all DataTypes in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class DataTypeIds
    {
        /// <summary>
        /// The identifier for the Book DataType.
        /// </summary>
        public static readonly ExpandedNodeId Book = new ExpandedNodeId(TPUM.DataTypes.Book, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Author DataType.
        /// </summary>
        public static readonly ExpandedNodeId Author = new ExpandedNodeId(TPUM.DataTypes.Author, TPUM.Namespaces.TPUM);
    }
    #endregion

    #region Variable Node Identifiers
    /// <summary>
    /// A class that declares constants for all Variables in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <summary>
        /// The identifier for the Book_Title Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Title = new ExpandedNodeId(TPUM.Variables.Book_Title, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Book_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Id = new ExpandedNodeId(TPUM.Variables.Book_Id, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Book_Authors Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Authors = new ExpandedNodeId(TPUM.Variables.Book_Authors, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Author_Books Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_Books = new ExpandedNodeId(TPUM.Variables.Author_Books, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Author_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_Id = new ExpandedNodeId(TPUM.Variables.Author_Id, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Author_FirstName Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_FirstName = new ExpandedNodeId(TPUM.Variables.Author_FirstName, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Author_LastName Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_LastName = new ExpandedNodeId(TPUM.Variables.Author_LastName, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the Author_NickName Variable.
        /// </summary>
        public static readonly ExpandedNodeId Author_NickName = new ExpandedNodeId(TPUM.Variables.Author_NickName, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the TPUM_XmlSchema Variable.
        /// </summary>
        public static readonly ExpandedNodeId TPUM_XmlSchema = new ExpandedNodeId(TPUM.Variables.TPUM_XmlSchema, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the TPUM_XmlSchema_NamespaceUri Variable.
        /// </summary>
        public static readonly ExpandedNodeId TPUM_XmlSchema_NamespaceUri = new ExpandedNodeId(TPUM.Variables.TPUM_XmlSchema_NamespaceUri, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the TPUM_BinarySchema Variable.
        /// </summary>
        public static readonly ExpandedNodeId TPUM_BinarySchema = new ExpandedNodeId(TPUM.Variables.TPUM_BinarySchema, TPUM.Namespaces.TPUM);

        /// <summary>
        /// The identifier for the TPUM_BinarySchema_NamespaceUri Variable.
        /// </summary>
        public static readonly ExpandedNodeId TPUM_BinarySchema_NamespaceUri = new ExpandedNodeId(TPUM.Variables.TPUM_BinarySchema_NamespaceUri, TPUM.Namespaces.TPUM);
    }
    #endregion

    #region BrowseName Declarations
    /// <summary>
    /// Declares all of the BrowseNames used in the Model Design.
    /// </summary>
    public static partial class BrowseNames
    {
        /// <summary>
        /// The BrowseName for the Author component.
        /// </summary>
        public const string Author = "Author";

        /// <summary>
        /// The BrowseName for the Authors component.
        /// </summary>
        public const string Authors = "Authors";

        /// <summary>
        /// The BrowseName for the Book component.
        /// </summary>
        public const string Book = "Book";

        /// <summary>
        /// The BrowseName for the Books component.
        /// </summary>
        public const string Books = "Books";

        /// <summary>
        /// The BrowseName for the FirstName component.
        /// </summary>
        public const string FirstName = "FirstName";

        /// <summary>
        /// The BrowseName for the Id component.
        /// </summary>
        public const string Id = "Id";

        /// <summary>
        /// The BrowseName for the LastName component.
        /// </summary>
        public const string LastName = "LastName";

        /// <summary>
        /// The BrowseName for the NickName component.
        /// </summary>
        public const string NickName = "NickName";

        /// <summary>
        /// The BrowseName for the Title component.
        /// </summary>
        public const string Title = "Title";

        /// <summary>
        /// The BrowseName for the TPUM_BinarySchema component.
        /// </summary>
        public const string TPUM_BinarySchema = "TPUM";

        /// <summary>
        /// The BrowseName for the TPUM_XmlSchema component.
        /// </summary>
        public const string TPUM_XmlSchema = "TPUM";
    }
    #endregion

    #region Namespace Declarations
    /// <summary>
    /// Defines constants for all namespaces referenced by the model design.
    /// </summary>
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";

        /// <summary>
        /// The URI for the TPUMXsd namespace (.NET code namespace is 'TPUM').
        /// </summary>
        public const string TPUMXsd = "http://tpum.example.com";

        /// <summary>
        /// The URI for the TPUMXsd namespace (.NET code namespace is 'TPUM').
        /// </summary>
        public const string TPUMXsd = "http://tpum.example.com";
    }
    #endregion

}