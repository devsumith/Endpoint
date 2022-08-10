using DotLiquid;
using Endpoint.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Endpoint.Core.Services
{
    public class LiquidTemplateProcessor : ITemplateProcessor
    {
        public string[] Process(string[] template, IDictionary<string, object> tokens, string[] ignoreTokens = null)
        {
            Hash hash = default;

            try
            {
                if (ignoreTokens != null)
                {
                    var dictionary = ImmutableDictionary.CreateBuilder<string, object>();

                    foreach (var entry in tokens)
                    {
                        if (!ignoreTokens.Contains(entry.Key))
                        {
                            dictionary.Add(entry.Key, entry.Value);
                        }
                    }

                    hash = Hash.FromDictionary(dictionary);

                }
                else
                {
                    hash = Hash.FromDictionary(tokens);
                }

                var liquidTemplate = Template.Parse(string.Join(Environment.NewLine, template));

                return liquidTemplate.Render(hash).Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(nameof(LiquidTemplateProcessor));

                throw e;
            }
        }

        public string Process(string template, IDictionary<string, object> tokens)
        {
            try
            {
                var hash = Hash.FromDictionary(tokens);

                var liquidTemplate = Template.Parse(template);

                return liquidTemplate.Render(hash);
            }
            catch (Exception e)
            {
                Console.WriteLine(nameof(LiquidTemplateProcessor));

                throw e;
            }
        }

        public string Process(string[] template, dynamic model)
        {
            var dictionary = ConvertObjectToDictionary(model);

            string[] result = Process(template, dictionary, ignoreTokens: null);

            return string.Join(Environment.NewLine, result);
        }

        private static Dictionary<string, object> ConvertObjectToDictionary(object o)
        {
            try
            {
                var dictionary = new Dictionary<string, object>();

                var properties = o.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (var prop in properties)
                {
                    var propValue = prop.GetValue(o, null);

                    if (propValue != null)
                    {
                        var propType = propValue.GetType();

                        if (propType == typeof(List<string>))
                        {
                            var list = new List<string>();

                            foreach (var x in (propValue as IEnumerable<string>))
                            {
                                list.Add(x);
                            }

                            dictionary.Add(prop.Name, list);
                        }
                        else if (propType.IsGenericType && (propType.GetGenericTypeDefinition() == typeof(List<>)))
                        {
                            var list = new List<object>();

                            foreach (var x in (propValue as IEnumerable<object>))
                            {
                                list.Add(ConvertObjectToDictionary(x));
                            }

                            dictionary.Add(prop.Name, list);
                        }
                        else if (propType != typeof(int) && propType != typeof(string))
                        {
                            dictionary.Add(prop.Name, ConvertObjectToDictionary(propValue));
                        }
                        else
                        {

                            var tokens = new TokensBuilder()
                                .With(prop.Name, (Token)propValue.ToString())
                                .Build();

                            foreach (var token in tokens)
                            {
                                dictionary.Add(token.Key, token.Value);
                            }
                        }
                    }
                }

                return dictionary;
            }
            catch(TargetParameterCountException exception)
            {
                return new Dictionary<string, object>();
            }
        }
    }
}
