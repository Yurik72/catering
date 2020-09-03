using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringPro.Core
{
    public class Translit
    {
			public static String cyr2lat(char ch)
			{
				switch (ch)
				{
				case 'а': return "a";
				case 'б': return "b";
				case 'в': return "v";
				case 'г': return "g";
				case 'д': return "d";
				case 'е': return "e";
				case 'ё': return "je";
				case 'ж': return "zh";
				case 'з': return "z";
				case 'и': return "i";
				case 'й': return "y";
				case 'к': return "k";
				case 'л': return "l";
				case 'м': return "m";
				case 'н': return "n";
				case 'о': return "o";
				case 'п': return "p";
				case 'р': return "r";
				case 'с': return "s";
				case 'т': return "t";
				case 'у': return "u";
				case 'ф': return "f";
				case 'х': return "kh";
				case 'ц': return "c";
				case 'ч': return "ch";
				case 'ш': return "sh";
				case 'щ': return "jsh";
				case 'ъ': return "yy";
				case 'ы': return "ih";
				case 'ь': return "jh";
				case 'э': return "EH";
				case 'ю': return "ju";
				case 'я': return "ja";

				//ENG
				case 'a': return "a";
				case 'b': return "b";
				case 'c': return "c";
				case 'd': return "d";
				case 'e': return "e";
				case 'f': return "f";
				case 'g': return "g";
				case 'h': return "h";
				case 'i': return "i";
				case 'j': return "j";
				case 'k': return "k";
				case 'l': return "l";
				case 'm': return "m";
				case 'n': return "n";
				case 'o': return "o";
				case 'p': return "p";
				case 'q': return "q";
				case 'r': return "r";
				case 's': return "s";
				case 't': return "t";
				case 'u': return "u";
				case 'v': return "v";
				case 'w': return "w";
				case 'x': return "x";
				case 'y': return "y";
				case 'z': return "z";

				//ENG UPPER
				case 'A': return "A";
				case 'B': return "B";
				case 'C': return "C";
				case 'D': return "D";
				case 'E': return "E";
				case 'F': return "F";
				case 'G': return "G";
				case 'H': return "H";
				case 'I': return "I";
				case 'J': return "J";
				case 'K': return "K";
				case 'L': return "L";
				case 'M': return "M";
				case 'N': return "N";
				case 'O': return "O";
				case 'P': return "P";
				case 'Q': return "Q";
				case 'R': return "R";
				case 'S': return "S";
				case 'T': return "T";
				case 'U': return "U";
				case 'V': return "V";
				case 'W': return "W";
				case 'X': return "X";
				case 'Y': return "Y";
				case 'Z': return "Z";

				//UA
				case 'і': return "i";
				case 'є': return "ye";
				case 'ї': return "yi";


				//upper
				case 'А': return "A";
					case 'Б': return "B";
					case 'В': return "V";
					case 'Г': return "G";
					case 'Д': return "D";
					case 'Е': return "E";
					case 'Ё': return "JE";
					case 'Ж': return "ZH";
					case 'З': return "Z";
					case 'И': return "I";
					case 'Й': return "Y";
					case 'К': return "K";
					case 'Л': return "L";
					case 'М': return "M";
					case 'Н': return "N";
					case 'О': return "O";
					case 'П': return "P";
					case 'Р': return "R";
					case 'С': return "S";
					case 'Т': return "T";
					case 'У': return "U";
					case 'Ф': return "F";
					case 'Х': return "KH";
					case 'Ц': return "C";
					case 'Ч': return "CH";
					case 'Ш': return "SH";
					case 'Щ': return "JSH";
					case 'Ъ': return "HH";
					case 'Ы': return "IH";
					case 'Ь': return "JH";
					case 'Э': return "EH";
					case 'Ю': return "JU";
					case 'Я': return "JA";
				//UA
				case 'І': return "I";
				case 'Є': return "YE";
				case 'Ї': return "YI";

				//digits
				case '0':return "0";
				case '1': return"1";
				case '2': return "2";
				case '3': return "3";
				case '4': return "4";
				case '5': return"5";
				case '6': return "6";
				case '7': return "7";
				case '8': return "8";
				case '9': return "9";
				default: return "_";
				}
			}

			public static String cyr2lat(String s)
			{
				StringBuilder sb = new StringBuilder(s.Length * 2);
			s.ToCharArray().ToList().ForEach(ch => sb.Append(cyr2lat(ch)));

				return sb.ToString();
			}
		}
	}

