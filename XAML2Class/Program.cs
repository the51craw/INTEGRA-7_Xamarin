using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace XAML2Class
{
    class Program
    {
//        enum _next
//        {
//            CHILD,
//            SIBLING,
////            PARENT
//        }

        // Flag for skipping comment lines:
        static Boolean inComment = false;

        // In order to name unnamed items we add suffix numbers:
        static Int32 suffixNumber = 0;   // Very likely to be unnamed

        static List<String> objectDeclarations = new List<String>();
        static List<String> objectProperties = new List<String>();
        static List<String> gridrowDeclarations = new List<String>();

        static List<List<String>> Classes = new List<List<String>>();
        static List<String> ClassLines = new List<String>();
        static Node rootNode = null;
        static Node currentParent;
        static Node newNode;
        //static _next next = _next.CHILD;
        static List<Node> crumbTrail = new List<Node>();
        static String[] eventHandlerNames = new String[] { // To be completed as need arizes!!!
            "SelectionChanged",
            "DoubleTapped",
            "Click",
            "GotFocus",
            "Tapped",
            "ValueChanged",
            "SelectedIndexChanged",
            "TextChanged",
            "KeyUp",
            "KeyDown",
        };

        [STAThread]
        static void Main(string[] args)
        {
            ClassLines.Add("class Form");
            ClassLines.Add("{");

            StreamReader streamReader = File.OpenText(@"D:\Users\hbe_000\Projects\INTEGRA_7_Xamarin\XAML\EditStudioset.xaml");
            while (!streamReader.EndOfStream)
            {
                String line = streamReader.ReadLine();
                line = line.Trim();
                if (!String.IsNullOrEmpty(line))
                {
                    // Comment handling only understands comments that are one or more full lines:
                    if (line.StartsWith("<!--") && line.EndsWith("-->"))
                    {
                        // Do not handle this line!
                    }
                    else if (line.StartsWith("<!--"))
                    {
                        inComment = true;
                        // Do not handle this line!
                    }
                    else if (inComment && line.EndsWith("-->"))
                    {
                        inComment = false;
                        // Do not handle this line!
                    }
                    else if (!inComment)
                    {
                        line = ReplaceSpacesBetweenQuotes(line);
                        while (!line.EndsWith(">"))
                        {
                            line += " " + streamReader.ReadLine();
                        }
                        line = line.Trim();
                        if (!String.IsNullOrEmpty(line))
                        {
                            Console.WriteLine(line);
                            AddToClass(line.Trim());
                        }
                    }
                }
            }

            // Recursively traverse all nodes:
            MakeObjectDeclarations(rootNode.Children[0]);
            MakeColumnDeclarations(rootNode.Children[0]);
            MakeGridrowDeclaration(rootNode.Children[0]);
            PutAllInFile();


            //Console.Read();
        }

        /*
         * A node with a name that is not present in Classes as class name
         * causes a new class to be added in Classes using the new name.
         * When reading, we have a current class in a class hierarchy.
         * Keeping a crumb-trail of nested classes lets us back-trace
         * up in the hierarchy when one class is finished.
         */
        static void AddToClass(String line)
        {
            if (rootNode == null)
            {
                rootNode = new Node(null);
                currentParent = rootNode;
                newNode = rootNode;
                AddAttributesToNode(line);
                //next = _next.CHILD;
            }
            else
            {
                if (line.StartsWith("</"))
                {
                    // End of node:
                    currentParent = currentParent.Parent;
                    //next = _next.SIBLING;
                }
                else if (line.EndsWith("/>"))
                {
                    // Self-containing node:
                    //AddNode();
                    newNode = new Node(currentParent);
                    currentParent.Children.Add(newNode);
                    AddAttributesToNode(line);

                    // Next node, if any, is a sibling:
                    //next = _next.SIBLING;
                }
                else if (line.StartsWith("<x:String>") && line.EndsWith("</x:String>"))
                {
                    // Combobox string. Add as comboboxitem to current:
                    currentParent.ComboboxItems.Add(line.Replace("<x:String>", "").Replace("</x:String>", ""));

                    // Next node, if any, is a child node:
                    //next = _next.CHILD;
                }
                else
                {
                    // New sub-node:
                    //AddNode();
                    newNode = new Node(currentParent);
                    currentParent.Children.Add(newNode);
                    AddAttributesToNode(line);
                    currentParent = newNode;

                    // Next node, if any, is a child node:
                    //next = _next.CHILD;
                }
            }
        }

        //static void AddNode()
        //{
        //    switch (next)
        //    {
        //        case _next.CHILD:
        //            current.Children.Add(current = new Node(current));
        //            break;
        //        case _next.SIBLING:
        //            current.Parent.Children.Add(new Node(current));
        //            break;
        //        //case _next.PARENT:
        //        //    current = current.Parent;
        //        //    break;
        //    }
        //}

        static void AddAttributesToNode(String line)
        {
            String[] parts = line.Split(' ');
            newNode.Type = parts[0].Trim(new char[] { ' ', '<', '/', '>' });
            foreach (String part in parts)
            {
                if (!String.IsNullOrEmpty(part))
                {
                    String trimmedPart = part.Trim(new char[] { ' ', '<', '/', '>' });
                    if (trimmedPart.Contains("="))
                    {
                        String[] attributeParts = trimmedPart.Split('=');
                        KeyValuePair<String, String> attribute = new KeyValuePair<String, String>(attributeParts[0], attributeParts[1].Replace((char)0x07, ' '));
                        newNode.Attributes.Add(attribute);
                    }
                }
            }
            if (newNode.Type == "ColumnDefinition" || newNode.Type == "RowDefinition")
            {
                if (line.Contains("="))
                {
                    parts = line.Split('=');
                    newNode.Size = parts[1].Trim('>', '/', ' ');
                    if (newNode.Size == "*")
                    {
                        newNode.Size = "1";
                    }
                }
                else
                {
                    newNode.Size = "1";
                }
            }
            FinalizeNode();
        }

        /*
         * We use String.Split(' ') to get the individual attributes, but attribute values
         * can contain spaces as well. The values are between double quotes, so we can not
         * have spaces there. A bell character is hardly used today, so we substitute all 
         * spaces between double quotes with bell characters (0x07) and restores them just
         * before storing the attribute value.
         */
        static String ReplaceSpacesBetweenQuotes(String s)
        {
            String[] parts = s.Split('\"');
            String result = "";

            // Now every odd part are those between double quotes.
            for (Int32 i = 1; i < parts.Length; i += 2)
            {
                parts[i] = parts[i].Replace(' ', '\a');
            }

            // Put it back together again:
            for (Int32 i = 0; i < parts.Length; i++)
            {
                result += parts[i];
            }
            return result;
        }

        /*
         * Fills out:
            Name,
            EventHandler,
            Margin,
            Padding,
            HorizontalAlignment,
            VerticalAlignment

         */
        static void FinalizeNode()
        {
            String[] eventHandler;
            foreach (KeyValuePair<String, String> keyValuePair in newNode.Attributes)
            {
                if (keyValuePair.Key == "x:Name" || keyValuePair.Key == "Name")
                {
                    newNode.Name = keyValuePair.Value;
                }
                else if (keyValuePair.Key == "Margin")
                {
                    newNode.Margin = keyValuePair.Value;
                }
                else if (keyValuePair.Key == "Padding")
                {
                    newNode.Padding = keyValuePair.Value;
                }
                else if (keyValuePair.Key == "HorizontalAlignment")
                {
                    newNode.HorizontalAlignment = keyValuePair.Value;
                }
                else if (keyValuePair.Key == "VerticalAlignment")
                {
                    newNode.VerticalAlignment = keyValuePair.Value;
                }
                else
                {
                    for (Int32 i = 0; i < eventHandlerNames.Length; i++)
                    {
                        if (keyValuePair.Key == eventHandlerNames[i])
                        {
                            eventHandler = new String[2];
                            eventHandler[0] = eventHandlerNames[i];
                            eventHandler[1] = keyValuePair.Value;
                            newNode.EventHandler.Add(eventHandler);
                        }
                    }
                }
            }
            if (newNode.Type == "Grid.ColumnDefinitions")
            {
                newNode.Type = "ColumnDefinitionCollection";
            }
            if (String.IsNullOrEmpty(newNode.Name))
            {
                newNode.Name = newNode.Type.ToLower() + NumberToString(suffixNumber++);
                if (newNode.Name.StartsWith("grid."))
                {
                    newNode.Name = newNode.Name.Replace("grid.", "");
                }
            }
            if (newNode.Type.StartsWith("Grid."))
            {
                newNode.Type = newNode.Type.Replace("Grid.", "");
            }
        }

        static void MakeObjectDeclarations(Node node)
        {
            MakeObjectDeclaration(node);
            foreach (Node child in node.Children)
            {
                MakeObjectDeclarations(child);
            }
        }

        static void MakeObjectDeclaration(Node node)
        {
            String key = "";
            String value = "";

            // Add valid object declaration:
            if (node.Type != "Grid.ColumnDefinitions"
                && node.Type != "Grid.RowDefinitions"
                && node.Type != "RowDefinitions"
                && node.Type != "ColumnDefinitionCollection")
            {
                objectDeclarations.Add(node.Type + " " + node.Name + " = new " + node.Type + "();");

                // Add object properties:
                foreach (KeyValuePair<String, String> keyValuePair in node.Attributes)
                {
                    if (keyValuePair.Key != "x:Name"
                        && keyValuePair.Key != "x:Uid"
                        )
                    {
                        key = keyValuePair.Key.Replace("x:", "");
                        value = keyValuePair.Value;

                        switch (key.Trim())
                        {
                            case "Width":
                            case "Height":
                                if (node.Type == "ColumnDefinition" || node.Type == "RowDefinition")
                                {
                                    // new GridLength(6, GridUnitType.Star);
                                    if (value == "*")
                                    {
                                        value = "new GridLength(1, GridUnitType.Star)";
                                    }
                                    else if (value.Contains("*"))
                                    {
                                        value = value.Trim(new char[] { '*', ' ', ';' });
                                        try
                                        {
                                            value = "new GridLength(" + Int32.Parse(value) + ", GridUnitType.Star)";
                                        }
                                        catch (Exception e)
                                        {
                                            value = "Could not parse " + keyValuePair.Value;
                                        }
                                    }
                                }
                                break;
                            case "Margin":
                                //  new Thickness(2, 2, 2, 2)
                                value = "new Thickness(" + value + ")";
                                break;
                            case "":
                                break;
                            default:
                                break;
                        }
                        String v = FixValue(node.Name, key, value);
                        if (!String.IsNullOrEmpty(v))
                        {
                            objectProperties.Add(node.Name + "." + key + " = " + v + ";");
                        }
                    }
                }
            }
        }

        static void MakeColumnDeclarations(Node node)
        {
            MakeColumnDeclaration(node);
            foreach (Node child in node.Children)
            {
                MakeColumnDeclarations(child);
            }
        }

        static void MakeColumnDeclaration(Node node)
        {
            // Add any column definitions:
            if (node.Type == "ColumnDefinitionCollection")
            {
                //// Create the ColumnDefinitionCollection:
                //objectProperties.Add(node.Parent.Name + ".ColumnDefinitions = new ColumnDefinitionCollection();");

                // Create the GridRow objects:
                Int32 i = 1;
                List<String> names = new List<String>();
                List<String> sizes = new List<String>();
                foreach (Node columdefinition in node.Children)
                {
                    // MainStudioSet.ColumnDefinitions.Add()
                    objectProperties.Add(node.Parent.Name + ".ColumnDefinitions.Add(" + columdefinition.Name + ");");
                    if (i < node.Parent.Children.Count) // There are columndefinitions that does not correspond to nodes! (Empty lines at bottom of screen.)
                    {
                        names.Add(node.Parent.Children[i].Name);
                        sizes.Add(columdefinition.Size);
                        i++;
                    }
                }

                // node.Parent.Name.Children.Add((new GridRow(0, new View[] {cbEditTone_ParameterPages, cbEditTone_PartialSelector,
                //     cbEditTone_InstrumentCategorySelector, cbEditTone_KeySelector }, new byte[] { 255, 255, 255, 255 })).Row);
                String declaration = node.Parent.Name + ".Children.Add((new GridRow(0, new View[] { ";
                for (i = 0; i < names.Count(); i++)
                {
                    declaration += names[i] + ", ";
                }
                declaration = declaration.TrimEnd(' ', ',');
                declaration += " }, new byte[] {";
                for (i = 0; i < sizes.Count(); i++)
                {
                    declaration += sizes[i] + ", ";
                }
                declaration = declaration.TrimEnd(' ', ',');
                declaration += " })).Row);";
                //gridrowDeclarations.Add(declaration);
            }
            else if (node.Type == "Grid")
            {
                String declaration = node.Name + ".Children.Add((new GridRow(0, new View[] { ";
                foreach (Node child in node.Children)
                {
                    if (!child.Name.StartsWith("columndefinitions_") 
                        && !child.Name.StartsWith("rowdefinitions_") 
                        && !child.Name.StartsWith("columndefinitioncollection_"))
                    declaration += child.Name + ", ";
                }
                declaration = declaration.TrimEnd(' ', ',');
                declaration += " })).Row);";
                gridrowDeclarations.Add(declaration);
            }
        }

        static void MakeGridrowDeclaration(Node node)
        {
            //String childNodes = "";
            //String columnDefinitions = "";
            //foreach (Node child in node.Children)
            //{
            //    if (child.Name == "Grid.ColumnDefinitions")
            //    {
            //        foreach (Node subChild in child.Children)
            //        {
            //            if (subChild.)
            //            columnDefinitions += 
            //        }
            //    }
            //}
        }

        static String FixValue(String name, String key, String value)
        {
            Boolean quote = true;

            if (value == "Collapsed" || value == "Visible")
            {
                value = "Visibility." + value;
                quote = false;
            }
            else if (value == "Green"
                || value == "Stretch"
                || value == "Center"
                || value == "Black"
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                )
            {
                quote = false;
                value = "";
            }
            else if (value.Trim().StartsWith("new ")
                || value == "true"
                || value == "false"
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                || value == ""
                )
            {
                quote = false;
            }
            else if (name.Contains("Thickness") || name.Contains("Padding") || key.Contains("Thickness") || key.Contains("Padding"))
            {
                value = "new Thickness(" + value +")";
                quote = false;
            }
            //else if (name.StartsWith("rowdefinition_"))
            //{
            //    value = "new Thickness(" + value + ")";
            //    quote = false;
            //}
            else if (key == "Grid.Column" || key == "Grid.Row")
            {
                value = "";
                quote = false;
            }
            if (quote)
            {
                try
                {
                    Int32.Parse(value);
                }
                catch (Exception e)
                {
                    value = "\"" + value + "\"";
                }
            }
            return value;
        }

        static String NumberToString(Int32 number)
        {
            String s = number.ToString();
            while (s.Length < 4)
            {
                s = "0" + s;
            }
            return "_" + s;
        }

        static void PutAllInFile()
        {
            File.WriteAllLines(@"D:\Users\hbe_000\Projects\INTEGRA_7_Xamarin\XAML\EditStudiosetDeclarations.xaml.cs",
                objectDeclarations.ToArray());
            File.WriteAllLines(@"D:\Users\hbe_000\Projects\INTEGRA_7_Xamarin\XAML\EditStudiosetProperties.xaml.cs",
                objectProperties.ToArray());
            File.WriteAllLines(@"D:\Users\hbe_000\Projects\INTEGRA_7_Xamarin\XAML\EditStudiosetGridRowDeclarations.xaml.cs",
                gridrowDeclarations.ToArray());
        }
    }

    class Node
    {
        public Node Parent { get; set; }
        public String Type { get; set; }
        public String Name { get; set; }
        public List<String[]> EventHandler { get; set; }
        public String Margin { get; set; }
        public String Padding { get; set; }
        public String HorizontalAlignment { get; set; }
        public String VerticalAlignment { get; set; }
        public String Size { get; set; }
        public List<KeyValuePair<String, String>> Attributes { get; set; }
        public List<String> ComboboxItems { get; set; }
        public List<Node> Children { get; set; }

        public Node(Node Parent)
        {
            this.Parent = Parent;
            Name = "";
            EventHandler = new List<String[]>();
            Margin = "";
            Padding = "";
            HorizontalAlignment = "";
            VerticalAlignment = "";
            Attributes = new List<KeyValuePair<String, String>>();
            ComboboxItems = new List<String>();
            Children = new List<Node>();
        }
    }
}
