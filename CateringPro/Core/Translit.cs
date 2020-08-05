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

