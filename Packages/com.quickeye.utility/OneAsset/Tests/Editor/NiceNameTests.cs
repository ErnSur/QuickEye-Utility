using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

namespace OneAsset.Tests.Editor
{
    public class NiceNameTests
    {
        private static IEnumerable GetTestCases2()
        {
            var str = "S Dkk2a_";
            var results = 
                from e in Enumerable.Range(0, 1 << str.Length)
                let p = 
                    from b in Enumerable.Range(0, str.Length)
                    select(e & (1 << b)) == 0 ? (char?)null : str[b]
                select string.Join(string.Empty, p);
            return results;
        }
        private static IEnumerable GetTestCases()
        {
            yield return "className2Number";
            yield return "className2number";
            yield return "_className2number_";
            yield return "m_className2number_";
            yield return " _className2number_ ";
            yield return "_ className2number _";
            yield return "className22Number";
            yield return "className222Number";
            yield return "className22number";
            yield return "className222number";
            yield return "className23";
            yield return "clasSName23";
            yield return "clasSDKName23";
            yield return "Class_Name_23";
            yield return "SDKName";
            yield return "SDK_Name";
            yield return "sDK_Name";
            
            // Abbveriations
            yield return "G2A";
            yield return "SDK2";
            yield return "SDK22";
            yield return "SDK2a";
            yield return "SDK22a";
            
            yield return "SDk2";
            yield return "SDk22";
            yield return "SDk2a";
            yield return "SDk22a";
            
            yield return "SDK22a2a";
            yield return "SDK2a2a";
            yield return "SDk2";
            yield return "SDk22";
            yield return "SDk22a";
            yield return "SD K2";
            yield return "SD k2";
            yield return "SDK2a";
            yield return "SDk2A";
            yield return "SD2A";
            yield return "2 A2 a";
            yield return RandomString(10);
            yield return RandomString(20);
            yield return RandomString(40);
        }

        [TestCaseSource(nameof(GetTestCases))]
        [TestCaseSource(nameof(GetTestCases2))]
        public void Should_SameOutput(string input)
        {
            var expected = UnityEditor.ObjectNames.NicifyVariableName(input);
            var actual = NicifyVariableName(input);
            Assert.AreEqual(expected, actual);
            Debug.Log(expected);
            //Debug.Log($"For input:\"{input}\" The expected output is:\"{expected}\"\nBut was:\"{actual}\"");
        }

        private static string NicifyVariableName(string input)
        {
            var result = new StringBuilder();

            var prevIsLetter = false;
            var prevIsLetterUpper = false;
            var prevIsDigit = false;
            var prevIsStartOfWord = false;
            var prevIsNumberWord = false;
            for (var i = input.Length - 1; i >= 0; i--)
            {
                var currentChar = input[i];
                var currIsLetter = char.IsLetter(currentChar);
                if (i == 0 && currIsLetter)
                {
                    currentChar = char.ToUpper(currentChar);
                }
                var currIsLetterUpper = char.IsUpper(currentChar);
                var currIsDigit = char.IsDigit(currentChar);
                
                bool[] conditions = {
                    currIsLetter && !currIsLetterUpper && prevIsLetterUpper,
                    currIsLetter && prevIsLetterUpper && prevIsStartOfWord,
                    currIsLetter && prevIsStartOfWord,
                    currIsDigit && prevIsStartOfWord,
                    !currIsDigit && prevIsNumberWord,
                    currIsLetter && !currIsLetterUpper && prevIsDigit,
                };
                var isSpacer = currentChar == ' ' || currentChar == '_';
                if (!isSpacer && conditions.Any(c=>c))
                {
                    var conds = string.Join(",", 
                        conditions.Select((c, i) => (c,i)).Where(t => t.c).Select(t=>t.i));
                    Debug.Log($"Space for {currentChar}: {conds}");
                    result.Insert(0, " ");
                }

                result.Insert(0, currentChar);
                prevIsStartOfWord = currIsLetter && currIsLetterUpper && prevIsLetter && !prevIsLetterUpper;
                prevIsNumberWord=  currIsDigit && (prevIsLetter && !prevIsLetterUpper);

                prevIsLetterUpper = currIsLetter && currIsLetterUpper;
                prevIsLetter = currIsLetter;
                prevIsDigit = currIsDigit;
            }

            return result.ToString();
        }

        public static string RandomString(int length)
        {
            const string chars = "Aa2_ ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}