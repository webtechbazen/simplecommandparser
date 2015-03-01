using System;
using System.Collections.Generic;

namespace ScriptParser
{
    public class CommandParser
    {
        readonly string _input;
        List<string> _tokens;
        string _data;
        bool _previousWasData;
        bool _hadCommand;

        enum ParseState
        {
            Default,
            Escaped,
            Text,
            TextEscaped
        }

        void finishData()
        {
            if (_previousWasData)
            {
                if (_data.Length > 0)
                    _tokens.Add(_data);
                _previousWasData = false;
                _data = "";
            }
        }

        void appendData(char ch)
        {
            _data += ch;
            _previousWasData = true;
        }

        public CommandParser(string input)
        {
            _input = input;
        }

        public List<string> Parse()
        {
            _tokens = new List<string>();
            var state = ParseState.Default;
            _data = "";
            _previousWasData = false;
            _hadCommand = false;
            for (int i = 0; i < _input.Length; i++)
            {
                char ch = _input[i];
                switch (ch)
                {
                    case ' ':
                        switch (state)
                        {
                            case ParseState.Default:
                                if (!_hadCommand && _data.Length > 0)
                                {
                                    _hadCommand = true;
                                    finishData();
                                }
                                break;
                            case ParseState.Escaped:
                                appendData(ch);
                                state = ParseState.Default;
                                break;
                            case ParseState.Text:
                                appendData(ch);
                                break;
                            case ParseState.TextEscaped:
                                appendData(ch);
                                state = ParseState.Text;
                                break;
                        }
                        break;

                    case ',':
                        switch (state)
                        {
                            case ParseState.Default:
                                finishData();
                                break;
                            case ParseState.Escaped:
                                appendData(ch);
                                state = ParseState.Default;
                                break;
                            case ParseState.Text:
                                appendData(ch);
                                break;
                            case ParseState.TextEscaped:
                                appendData(ch);
                                state = ParseState.Text;
                                break;
                        }
                        break;

                    case '\\':
                        switch (state)
                        {
                            case ParseState.Default:
                                state = ParseState.Escaped;
                                break;
                            case ParseState.Escaped:
                                appendData(ch);
                                state = ParseState.Default;
                                break;
                            case ParseState.Text:
                                state = ParseState.TextEscaped;
                                break;
                            case ParseState.TextEscaped:
                                appendData(ch);
                                state = ParseState.Text;
                                break;
                        }
                        break;

                    case '\"':
                        switch (state)
                        {
                            case ParseState.Default:
                                state = ParseState.Text;
                                break;
                            case ParseState.Escaped:
                                appendData(ch);
                                state = ParseState.Default;
                                break;
                            case ParseState.Text:
                                state = ParseState.Default;
                                break;
                            case ParseState.TextEscaped:
                                appendData(ch);
                                state = ParseState.Text;
                                break;
                        }
                        break;

                    default:
                        if (state == ParseState.Escaped)
                            state = ParseState.Default;
                        appendData(ch);
                        break;
                }
            }
            finishData();
            return _tokens;
        }
    }
}