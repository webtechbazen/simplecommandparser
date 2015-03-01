using System;
using System.Collections.Generic;

namespace ScriptParser
{
    /// <summary>
    /// Command parser.
    /// </summary>
    public class Command
    {
        string _data;
        readonly List<String> _tokens = new List<string>();

        enum ParseState
        {
            Default,
            Escaped,
            Text,
            TextEscaped
        }

        void dataFinish()
        {
            if (_data.Length > 0)
            {
                _tokens.Add(_data);
                _data = "";
            }
        }

        void dataAppend(char ch)
        {
            _data += ch;
        }

        /// <summary>
        /// Get the command text.
        /// </summary>
        public string Text
        {
            get
            {
                return _tokens.Count > 0 ? _tokens[0] : "";
            }
        }

        /// <summary>
        /// Gets the command arguments.
        /// </summary>
        public string[] Arguments
        {
            get
            {
                var arguments = new string[_tokens.Count - 1];
                if (_tokens.Count > 1)
                    _tokens.CopyTo(1, arguments, 0, arguments.Length);
                return arguments;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptParser.Command"/> class and parses the command.
        /// </summary>
        /// <param name="command">The command to parse.</param>
        public Command(string command)
        {
            var state = ParseState.Default;
            foreach (char ch in command)
            {
                switch (state)
                {
                    case ParseState.Default:
                        switch (ch)
                        {
                            case ' ':
                                if (_tokens.Count == 0 && _data.Length > 0)
                                    dataFinish();
                                break;
                            case ',':
                                dataFinish();
                                break;
                            case '\\':
                                state = ParseState.Escaped;
                                break;
                            case '\"':
                                state = ParseState.Text;
                                break;
                            default:
                                dataAppend(ch);
                                break;
                        }
                        break;
                    case ParseState.Escaped:
                        dataAppend(ch);
                        state = ParseState.Default;
                        break;
                    case ParseState.Text:
                        switch (ch)
                        {
                            case '\\':
                                state = ParseState.TextEscaped;
                                break;
                            case '\"':
                                dataFinish();
                                state = ParseState.Default;
                                break;
                            default:
                                dataAppend(ch);
                                break;
                        }
                        break;
                    case ParseState.TextEscaped:
                        dataAppend(ch);
                        state = ParseState.Text;
                        break;
                }
            }
            dataFinish();
        }
    }
}