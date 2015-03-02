using System;
using System.Collections.Generic;

namespace ScriptParser
{
    public class Parentheses
    {
        class Element
        {
            public Element(Element parent)
            {
                this.parent = parent;
            }

            public Element parent;

            public List<Element> children = new List<Element>();

            public string val = "";
        }

        Element root;

        void printRecursive(Element element, int depth, ref string result)
        {
            string spacing = "";
            for (int i = 0; i < depth * 4; i++)
                spacing += "-";

            Console.WriteLine(String.Format("{0}Element: {1}", 
                    spacing, element == root ? "root" : element.val));
            foreach (var child in element.children)
            {
                if (child.val == "")
                    result += "(";
                printRecursive(child, depth + 1, ref result);
                if (child.val == "")
                    result += ")";
            }
            if (element.val != "")
                result += element.val;
        }

        /// <summary>
        /// Print the tree to the console.
        /// </summary>
        public void Print()
        {
            if (root == null)
            {
                Console.WriteLine("Tree construction failed..");
            }
            else
            {
                string result = "";
                printRecursive(root, 0, ref result);
            }
        }

        /// <summary>
        /// Constructs a tree from expressions like this: "((2*4-6/3)*(3*5+8/4))-(2+3)".
        /// </summary>
        /// <param name="input">Expression to construct a tree from.</param>
        public Parentheses(string input)
        {
            try
            {
                var stack = new Stack<Element>();
                stack.Push(root = new Element(null));
                Element child = null;
                foreach (char ch in input)
                {
                    var peek = stack.Peek();
                    switch (ch)
                    {
                        case '(':
                            if (child != null) //there was text on the same depth
                            {
                                peek.children.Add(child);
                                child = null;
                            }
                            var newElement = new Element(peek);
                            peek.children.Add(newElement);
                            stack.Push(newElement);
                            break;
                        case ')':
                            if (child != null) //there was text on the same depth
                            {
                                peek.children.Add(child);
                                child = null;
                            }
                            if (stack.Pop() == root) //happens when you parse "5*(7+1))"
                                throw new Exception("Popping the root element is not allowed.");
                            break;
                        default:
                            if (child == null) //only create a child when text is encountered
                                child = new Element(peek);
                            child.val += ch;
                            break;
                    }
                }
                if (child != null)
                    stack.Pop().children.Add(child);
            }
            catch
            {
                root = null;
            }
        }
    }
}