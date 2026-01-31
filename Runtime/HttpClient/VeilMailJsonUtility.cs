using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace VeilMail.Http
{
    /// <summary>
    /// Lightweight JSON utility for Unity. No external dependencies.
    /// Handles Dictionary&lt;string, object&gt;, List&lt;object&gt;, string, double, long, bool, null.
    /// </summary>
    public static class VeilMailJsonUtility
    {
        public static Dictionary<string, object> Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json)) return new Dictionary<string, object>();
            var parser = new JsonParser(json);
            var result = parser.ParseValue();
            return result as Dictionary<string, object> ?? new Dictionary<string, object>();
        }

        public static string Serialize(object obj)
        {
            var sb = new StringBuilder();
            SerializeValue(obj, sb);
            return sb.ToString();
        }

        private static void SerializeValue(object value, StringBuilder sb)
        {
            if (value == null)
            {
                sb.Append("null");
            }
            else if (value is string s)
            {
                sb.Append('"');
                foreach (var c in s)
                {
                    switch (c)
                    {
                        case '"': sb.Append("\\\""); break;
                        case '\\': sb.Append("\\\\"); break;
                        case '\n': sb.Append("\\n"); break;
                        case '\r': sb.Append("\\r"); break;
                        case '\t': sb.Append("\\t"); break;
                        default: sb.Append(c); break;
                    }
                }
                sb.Append('"');
            }
            else if (value is bool b)
            {
                sb.Append(b ? "true" : "false");
            }
            else if (value is int || value is long || value is float || value is double)
            {
                sb.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
            }
            else if (value is Dictionary<string, object> dict)
            {
                sb.Append('{');
                var first = true;
                foreach (var kvp in dict)
                {
                    if (!first) sb.Append(',');
                    first = false;
                    SerializeValue(kvp.Key, sb);
                    sb.Append(':');
                    SerializeValue(kvp.Value, sb);
                }
                sb.Append('}');
            }
            else if (value is List<object> list)
            {
                sb.Append('[');
                for (var i = 0; i < list.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    SerializeValue(list[i], sb);
                }
                sb.Append(']');
            }
            else
            {
                SerializeValue(value.ToString(), sb);
            }
        }

        private class JsonParser
        {
            private readonly string _json;
            private int _pos;

            public JsonParser(string json) { _json = json; _pos = 0; }

            public object ParseValue()
            {
                SkipWhitespace();
                if (_pos >= _json.Length) return null;

                switch (_json[_pos])
                {
                    case '{': return ParseObject();
                    case '[': return ParseArray();
                    case '"': return ParseString();
                    case 't': case 'f': return ParseBool();
                    case 'n': _pos += 4; return null;
                    default: return ParseNumber();
                }
            }

            private Dictionary<string, object> ParseObject()
            {
                var dict = new Dictionary<string, object>();
                _pos++; // skip {
                SkipWhitespace();
                if (_pos < _json.Length && _json[_pos] == '}') { _pos++; return dict; }

                while (_pos < _json.Length)
                {
                    SkipWhitespace();
                    var key = ParseString();
                    SkipWhitespace();
                    _pos++; // skip :
                    var value = ParseValue();
                    dict[key] = value;
                    SkipWhitespace();
                    if (_pos < _json.Length && _json[_pos] == ',') { _pos++; continue; }
                    break;
                }
                if (_pos < _json.Length && _json[_pos] == '}') _pos++;
                return dict;
            }

            private List<object> ParseArray()
            {
                var list = new List<object>();
                _pos++; // skip [
                SkipWhitespace();
                if (_pos < _json.Length && _json[_pos] == ']') { _pos++; return list; }

                while (_pos < _json.Length)
                {
                    list.Add(ParseValue());
                    SkipWhitespace();
                    if (_pos < _json.Length && _json[_pos] == ',') { _pos++; continue; }
                    break;
                }
                if (_pos < _json.Length && _json[_pos] == ']') _pos++;
                return list;
            }

            private string ParseString()
            {
                _pos++; // skip opening quote
                var sb = new StringBuilder();
                while (_pos < _json.Length && _json[_pos] != '"')
                {
                    if (_json[_pos] == '\\')
                    {
                        _pos++;
                        switch (_json[_pos])
                        {
                            case '"': sb.Append('"'); break;
                            case '\\': sb.Append('\\'); break;
                            case 'n': sb.Append('\n'); break;
                            case 'r': sb.Append('\r'); break;
                            case 't': sb.Append('\t'); break;
                            case '/': sb.Append('/'); break;
                            default: sb.Append(_json[_pos]); break;
                        }
                    }
                    else
                    {
                        sb.Append(_json[_pos]);
                    }
                    _pos++;
                }
                _pos++; // skip closing quote
                return sb.ToString();
            }

            private bool ParseBool()
            {
                if (_json[_pos] == 't') { _pos += 4; return true; }
                _pos += 5; return false;
            }

            private object ParseNumber()
            {
                var start = _pos;
                var isFloat = false;
                while (_pos < _json.Length && "0123456789.eE+-".IndexOf(_json[_pos]) >= 0)
                {
                    if (_json[_pos] == '.' || _json[_pos] == 'e' || _json[_pos] == 'E')
                        isFloat = true;
                    _pos++;
                }
                var numStr = _json.Substring(start, _pos - start);
                if (isFloat)
                    return double.Parse(numStr, CultureInfo.InvariantCulture);
                if (long.TryParse(numStr, out var l))
                    return l;
                return double.Parse(numStr, CultureInfo.InvariantCulture);
            }

            private void SkipWhitespace()
            {
                while (_pos < _json.Length && char.IsWhiteSpace(_json[_pos])) _pos++;
            }
        }
    }
}
