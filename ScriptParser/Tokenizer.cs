using System;
using System.Collections.Generic;

namespace ScriptParser
{
    public class Tokenizer
    {
        readonly string input;

        enum State
        {
            None,
            Text,
            Escaped
        }

        public Tokenizer(string input)
        {
            this.input = input;
        }

        List<string> _tokens;
        string _data;
        bool _previousWasData;
        bool _hadCommand;

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

        public List<string> Tokenize()
        {
            _tokens = new List<string>();
            var state = new Stack<State>();
            state.Push(State.None);
            _data = "";
            _previousWasData = false;
            _hadCommand = false;
            for (int i = 0; i < input.Length; i++)
            {
                var curState = state.Peek();
                char ch = input[i];
                switch (ch)
                {
                    case ' ':
                        switch (curState)
                        {
                            case State.None:
                                if (!_hadCommand && _data.Length > 0)
                                {
                                    _hadCommand = true;
                                    finishData();
                                }
                                break;
                            case State.Escaped:
                                appendData(ch);
                                state.Pop();
                                break;
                            case State.Text:
                                appendData(ch);
                                break;
                        }
                        break;

                    case ',':
                        switch (curState)
                        {
                            case State.None:
                                finishData();
                                break;
                            case State.Escaped:
                                appendData(ch);
                                state.Pop();
                                break;
                            case State.Text:
                                appendData(ch);
                                break;
                        }
                        break;

                    case '\\':
                        switch (curState)
                        {
                            case State.Text:
                            case State.None:
                                state.Push(State.Escaped);
                                break;
                            case State.Escaped:
                                appendData(ch);
                                state.Pop();
                                break;
                        }
                        break;

                    case '\"':
                        switch (curState)
                        {
                            case State.None:
                                state.Push(State.Text);
                                break;
                            case State.Escaped:
                                appendData(ch);
                                state.Pop();
                                break;
                            case State.Text:
                                state.Pop();
                                break;
                        }
                        break;

                    default:
                        if (curState == State.Escaped)
                            state.Pop();
                        appendData(ch);
                        break;
                }
            }
            finishData();
            return _tokens;
        }
    }
}