using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
 namespace Compilador
{
	class Analizador_Lexico
	{
		Archivos file;
		//tabla de token
		ArrayList tokens = new ArrayList();
		Token token;
		Error error;
		ArrayList errores = new ArrayList();
		int i = 0;//indice para el numero de token para analizador de sintaxis
		ArrayList variables = new ArrayList();//almacena variables declaradas
		public Analizador_Lexico()
		{
			file = new Archivos();
		}


		public Archivos File
		{
			get { return this.file; }
			set { this.file = value; }
		}

		//análisis léxico
		public void matriz_transicion(string nombre_archivo)
		{           //matriz de transición
			int[,] matriz = new int[,]
	{            /* L    N    E    e     _    .    "    '    ,    ;    +   -   /     *    ^    %    >    <    !     =   (     )   {    }    [    ]     &    |   \n   \r   \s   \t  \eof   oc       */               
                {    1,   8,   1,   1,  -5,  -5,  23,  24, 108, 109,  15,  16,   4, 114, 115, 116,  17,  18,  19,  20, 125, 126, 127, 128, 129, 130,  21,  22,   0,   0,   0,   0,   0,  -5 }, //0
                {    1,   2,   1,   1,   3, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101,  101, 101}, //1
                {    1,   2,   1,   1,   3, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101,  101, 101}, //2
                {    1,   2,   1,   1,   3, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101,  101, 101}, //3
                {  102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102,   5,   6, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102,  102, 102}, //4
                {    5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   5,   0,   0,   5,  5 ,    0,   0}, //5
                {    6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   7,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,  6 ,    0,   0}, //6
                {    6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   0,   7,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,  6 ,    0,   0}, //7
                {  103,   8,   9,   9, 103,  10, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103, 103,  103, 103}, //8
                {   -1,  11,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  13,  12,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,   -1,  -1}, //9
                {   -2,  14,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,  -2,   -2,  -2}, //10
                {  104,  11, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104, 104,  104, 104}, //11
                {   -1,  11,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, -1 ,   -1,  -1}, //12
                {   -1,  11,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1,  -1, -1 ,   -1,  -1}, //13
                {  105,  14,   9,   9, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105, 105,  105, 105}, //14
                {  110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 111, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110, 110,  110, 110}, //15
                {  112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 113, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112, 112,  112, 112}, //16
                {  117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 118, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117, 117,  117, 117}, //17
                {  119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 120, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119, 119,  119, 119}, //18
                {  121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 122, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121, 121,  121, 121}, //19
                {  123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 124, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123,  123, 123}, //20
                {   -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3,  -3, 131,  -3,  -3,  -3,  -3, -3 ,   -3,  -3}, //21
                {   -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4,  -4, 132,  -4,  -4,  -4, -4 ,   -4,  -4}, //22
                {   23,  23,  23,  23,  23,  23, 106,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,  23,   -6,  23}, //23
                {   25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  25,  -7,  -7,  25,  -7,   -7,  -7}, //24
                {   -8,  -8,  -8,  -8,  -8,  -8,  -8, 107,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,  -8,   -8,  -8}, //25

	};


			//leer archivo
			string line; //almacena la linea leída del archivo
			char[] cadena; // se guardará la linea leida por caracteres 
			char caracter = ' ';
			int cont = 0, end = 0; //guardaremos el número de caracter en el que nos encontramos para añadirlo a la tabla token
			int estado = 0; //almacenaremos el estado en el que nos encontramos

			int c = 0; //representa los caracteres en la matriz de trancision (columnas)
			int contarlinea = 0;

			string simbolo = "";
			bool leer = true; //comprobaremos la lectura para evitar perder tokens generados     
			bool terminar = false;
			//inicio del texto
			file.Nombre = nombre_archivo;
			File.AbrirLeer();

			while ((line = File.Leer()) != null)
			{
				contarlinea++;
				cadena = line.ToCharArray();
				cont = 0;
				end = cadena.Length;
				leer = true;
				terminar = false;

				//while (cont < cadena.Length) {

				while(terminar == false)
				{

					if (cont < cadena.Length && leer == true) { caracter = cadena[cont]; cont++; terminar = false; }
					else if (leer == true) { caracter = '\n'; terminar = true; }
					//if (cont == cadena.Length) { }
					//leer o no leer

					if (caracter >= '\u0041' && caracter <= '\u005A') { if (caracter == '\u0045') { c = 2; } else { c = 0; } } //letras may
					if (caracter >= '\u0061' && caracter <= '\u007A') { if (caracter == '\u0065') { c = 3; } else { c = 0; } } //letras min
					if (caracter >= '\u0030' && caracter <= '\u0039') { c = 1; } //numeros
					switch (caracter)
					{
						case '\u005F': { c = 4; break; } //guion bajo _   
						case '\u002E': { c = 5; break; } //punto .
						case '\u0022': { c = 6; break; } //comillas "
						case '\u0027': { c = 7; break; } //apóstrofe '
						case '\u002C': { c = 8; break; } //coma ,
						case '\u003B': { c = 9; break; } // punto y coma ;
						case '\u002B': { c = 10; break; } //suma +
						case '\u002D': { c = 11; break; } //resta - o guion
						case '\u002F': { c = 12; break; } // diagonal /
						case '\u002A': { c = 13; break; } //asterisco *
						case '\u005E': { c = 14; break; } //potencia ^
						case '\u0025': { c = 15; break; } //porcentaje %
						case '\u003E': { c = 16; break; } //mayor que >
						case '\u003C': { c = 17; break; } //menor que <
						case '\u0021': { c = 18; break; } //exclamación !
						case '\u003D': { c = 19; break; } //igualdad =
						case '\u0028': { c = 20; break; } //parentesis (
						case '\u0029': { c = 21; break; } //parentesis )
						case '\u007B': { c = 22; break; } // llave {
						case '\u007D': { c = 23; break; } // llave }
						case '\u005B': { c = 24; break; } //corchete [
						case '\u005D': { c = 25; break; } //corchete ]
						case '\u0026': { c = 26; break; } //ampersand &
						case '\u007C': { c = 27; break; } //barra |                               
						case '\n': { c = 28; break; } //salto de línea \n u000A
						case '\u000D': { c = 29; break; } //retorno de carro \r
						case '\u0020': { c = 30; break; } // espacio en blanco o u0020
						case '\u0009': { c = 31; break; } //tab \t
						case '\u0003': { c = 32; break; } //final del texto o end of file
						case '\u0023': { c = 33; break; } // #
						case '\u0024': { c = 33; break; } // $
						case '\u003F': { c = 33; break; } // ?
						case '\u0040': { c = 33; break; } // @
						case '\u0060': { c = 33; break; } // `
						case '\u007E': { c = 33; break; } // ~
						case '\u00AC': { c = 33; break; } // ¬
						case '\u00B0': { c = 33; break; } // °
						case '\u00A8': { c = 33; break; } // ¨
						case '\u00B4': { c = 33; break; } // ´
						case '\u00BF': { c = 33; break; } // ¿
						case '\u00D1': { c = 33; break; } // Ñ
						case '\u00F1': { c = 33; break; } // ñ 
														  //default: { c = 33; break; }

					}

					estado = matriz[estado, c];

					if (estado < 0)
					{
						error = new Error();
						error.No_error = estado;
						error.Columna = cont;
						error.Fila = contarlinea;
						error.Simbolo = Convert.ToString(caracter);
						errores.Add(error);
						estado = 0;
					}


					if (estado != 101 && estado != 102 && estado != 103 && estado != 104 && estado != 105 && estado != 110 &&
						estado != 112 && estado != 117 && estado != 119 && estado != 121 && estado != 123)
					{ leer = true; }
					else { leer = false; }

					if (leer == true)
					{
						if (estado > 0)
						{
							if (caracter != '\u0020' && caracter != '\u000A' && caracter != '\u0009' && caracter != '\u000D')
							{ simbolo = simbolo + caracter; }
						}
					}
					if (leer == true)
						Console.Write(caracter);

					//conver = new string(simbolo); //convertiremos el simbolo token guardado en el arreglo de chars a string.

					if (estado > 100)
					{
						token = new Token();
						token.Renglon = contarlinea;
						token.Columna = cont;
						if (Reserve(simbolo) != 0)
						{ token.No_token = Reserve(simbolo); }
						else { token.No_token = estado; }
						token.Simbolo = simbolo;
						tokens.Add(token);

						estado = 0;
						simbolo = "";//regresamos a la posicion 0 del arreglo simbolo
						if (cont == cadena.Length && leer == false) { cont = cadena.Length - 1; leer = true; } //else { cont++; }

					}




				} 



			} //fin del texto


			file.CerrarLeer();

			Console.WriteLine("\nR  C Token  Sim");

			foreach (Token token in tokens)
			{
				Console.WriteLine
		 (token.Renglon + "  " + token.Columna + "  " + token.No_token + "   " + token.Simbolo);

			}

			Console.WriteLine("\nE  C  F  Sim");

			foreach (Error error in errores)
			{
				Errores(error.No_error);
				Console.WriteLine
		 (error.No_error + "  " + error.Columna + "  " + error.Fila + "   " + error.Simbolo);

			}

			estructura();
			System.Console.ReadLine();

		}//dfin edl metodo


		//análisis sintáctico 
		public void estructura()
		{
			int last = tokens.Count - 1;//último token registrado



			if (((Token)(tokens[i])).No_token != 150) { Console.WriteLine("Error de sintaxis. Se esperaba el nombre namespace."); return; }
			if (i < last) { i++; }	
			if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis.Se esperaba definición del espacio."); return; }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba llave de inicio del bloque."); return; }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token == 134 || ((Token)(tokens[i])).No_token == 135 || ((Token)(tokens[i])).No_token == 136 || ((Token)(tokens[i])).No_token == 137 || ((Token)(tokens[i])).No_token == 151)
			{ dec_var(); } //Se identifica la declaración de algún tipo de dato, existe una declaración de variables.
			if (((Token)(tokens[i])).No_token == 133) { principal(((Token)(tokens[i])).No_token); } //si se identifica el token de main, se ignora la declaración de variables.
			if (i < last) { i++; }
			if ((((Token)tokens[last])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba llave de cierre del bloque."); }




			Console.Write(tokens.Count);
		}

		
		// bloque principal main{}
		public void principal(int token)
		{
			int last = tokens.Count - 1;//último token registrado


			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
			if (i < last) { i++; }
			instrucciones(token);
			if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }


		}
		public bool exp_comp() {
			int last = tokens.Count - 1;//último token registrado

			bool ban = false;
			
			if (expresion() != true) { Console.WriteLine("Error de sintaxis."); return false; }
			ban = true;
			if (i < last) { i++; }
			
			do
			{
				if (((Token)(tokens[i])).No_token != 131 || ((Token)(tokens[i])).No_token != 132)
				{
					ban = true;
				}
				else
				{
				
					if (expresion() != true)
					{ Console.WriteLine("Error de sintaxis."); return false; }
					else
					{
						if (i < last) { i++; }
						ban = false; }

				}
			} while (ban == false);

			return ban;
		}
		public bool expresion() {
			int last = tokens.Count - 1;//último token registrado

			if (exp_simp() != true) { Console.WriteLine("Error de sintaxis."); return false; }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token == 124 || ((Token)(tokens[i])).No_token == 117 || ((Token)(tokens[i])).No_token == 119 || ((Token)(tokens[i])).No_token == 120 || ((Token)(tokens[i])).No_token == 118 || ((Token)(tokens[i])).No_token == 122 )
			{
				
				if (exp_simp() != true) { Console.WriteLine("Error de sintaxis."); } }

			return true;
		}
		public bool exp_simp()
		{
			int last = tokens.Count - 1;//último token registrado

			bool ban = false;
		
			if (termino() != true) { Console.WriteLine("Error de sintaxis."); return false; }
			if (i < last) { i++; }

			do
			{
					if (((Token)(tokens[i])).No_token != 110 || ((Token)(tokens[i])).No_token != 112)
					{ ban = true; }
					else
					{
					
						if (termino() != true)
						{ Console.WriteLine("Error de sintaxis."); return false; }
						else
					{
						if (i < last) { i++; }
						ban = false; }
					}
				} while (ban==false);

			return ban;
		}
		public bool termino()
		{
			int last = tokens.Count - 1;//último token registrado

			bool ban = false;
			
			if (factor() != true) { Console.WriteLine("Error de sintaxis."); return false; }
			
				do
				{ 
				if (((Token)(tokens[i])).No_token != 102 || ((Token)(tokens[i])).No_token != 114)
				{
					ban= true;
				}
				else
				{
					factor();
					if (factor() != true)
					{ Console.WriteLine("Error de sintaxis."); return false; }
					else
					{
						if (i < last) { i++; }
						ban = false; }
				}
			    }while (ban==false) ;
				return ban;
			
	    }
		public bool factor() {
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token == 101 || ((Token)(tokens[i])).No_token == 103 || ((Token)(tokens[i])).No_token == 104 || ((Token)(tokens[i])).No_token == 105
				|| ((Token)(tokens[i])).No_token == 106 || ((Token)(tokens[i])).No_token == 107 || ((Token)(tokens[i])).No_token == 125 || ((Token)(tokens[i])).No_token == 129)
			{
				if (((Token)(tokens[i])).No_token == 125 || ((Token)(tokens[i])).No_token == 129)
				{
					if (i < last) { i++; }
					expresion();
					if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida."); return false; }

					if (i < last) { i++; }

					if (((Token)(tokens[i])).No_token != 126 || ((Token)(tokens[i])).No_token != 130)
					{ Console.WriteLine("Error de sintaxis. Se esperaba ( o ]"); return false; }
					else
					{
						if (i < last) { i++; }
						return true; }
				}
				return true;
			}
			else{ Console.WriteLine("Error de sintaxis."); return false; }

			//return true;
		}
		//instrucciones
		public void dec_var()
		{
			int last = tokens.Count - 1;
			bool ban = false;
			do
			{
				if (((Token)(tokens[i])).No_token == 134 || ((Token)(tokens[i])).No_token == 135 || ((Token)(tokens[i])).No_token == 136
					|| ((Token)(tokens[i])).No_token == 137 || ((Token)(tokens[i])).No_token == 151)
				{ ban = true;
					if (i < last) { i++; }
					if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba identificador."); return; }
					variables.Add(((Token)(tokens[i])));
					if (i < last) { i++; }
					if (((Token)(tokens[i])).No_token != 123) { Console.WriteLine("Error de sintaxis. Se esperaba símbolo de asignación."); return; }
					if (i < last) { i++; }
					if (expresion() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida"); return; }
					if (i < last) { i++; }
					if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba punto y coma."); return; }
					if (i < last) { i++; }

				}
				else { ban = false; }

			} while (ban == true); 
		}

		public void instrucciones(int token)
		{
			switch (token)
			{
				case 101: { asignacion(); break; }
				case 141: { cond_if(); break; }
				case 142: { ciclo_for(); break; }
				case 143: { ciclo_while(); break; }
				case 144: { cond_switch(); break; }
				case 146: { ciclo_dowhile(); break; }
				case 148: { escribir(); break; }
				case 149: { leer(); break; }		  

			}

		}
		public void asignacion()
		{
			int last = tokens.Count - 1;

			//preguntamos si la variable existe 
			if (variables.Contains(((Token)(tokens[i])).Simbolo)!=true) { Console.WriteLine("Error de semántica. La variable no ha sido declarada anteriormente."); }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token != 123) { Console.WriteLine("Error de sintaxis. Se esperaba el símbolo ="); }
			if (i < last) { i++; }
			expresion();
			if (expresion() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida."); }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
			if (i < last) { i++; }


		}
		public void ciclo_for() {
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba identificador "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 123) { Console.WriteLine("Error de sintaxis. Se esperaba '=' "); }
			if (i < last) { i++; }

			expresion();
			if (((Token)(tokens[i])).No_token != 101 ||expresion()!=true ) { Console.WriteLine("Error de sintaxis. Asignación inválida. "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
			if (i < last) { i++; }

			expresion();
			if (expresion()!=true) { Console.WriteLine("Error de sintaxis. Expresión inválida "); }
			
			if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
			if (i < last) { i++; }
			expresion();
			if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
			if (i < last) { i++; }

			bloque_instrucciones();
			if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }

			if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
			if (i < last) { i++; }

		}
		public void ciclo_while()
		{
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }

			expresion();
			if (expresion() != true) { Console.WriteLine("Error de sintaxis. Expresión inválida "); }
			
			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
			if (i < last) { i++; }

			bloque_instrucciones();
			if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }

			if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
			if (i < last) { i++; }


		}
		public void ciclo_dowhile() {
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
			if (i < last) { i++; }

			bloque_instrucciones();
			if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
			if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 143) { Console.WriteLine("Error de sintaxis. Se esperaba la palabra 'while' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }

			exp_comp();
			if (exp_comp() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida."); }

			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
			if (i < last) { i++; }

		}
		public void cond_if()
		{
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }

			exp_comp();
			if (exp_comp() != true) { Console.WriteLine("Error de sintaxis. La expresión asignada es inválida."); }

			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
			if (i < last) { i++; }

			bloque_instrucciones();
			if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
			if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
			if (i < last) { i++; }

			//else
			if (((Token)(tokens[i])).No_token != 140)
			{
				if (i < last) { i++; }

				if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
				if (i < last) { i++; }

				bloque_instrucciones();
				if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
				if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
			}
			if (i < last) { i++; }

		}
		public void cond_switch() {
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 101) { Console.WriteLine("Error de sintaxis. Se esperaba identificador "); }
			//preguntamos si la variable existe 
			if (variables.Contains(((Token)(tokens[i])).Simbolo) != true) { Console.WriteLine("Error de semántica. La variable no ha sido declarada anteriormente."); }
			if (i < last)
			{
				if (i < last) { i++; }
			}
			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
			if (i < last) { i++; }

			bool ban = true;
			do
			{
				if (((Token)(tokens[i])).No_token != 152) { Console.WriteLine("Error de sintaxis. Se esperaba la palabra case. "); ban = true; }
				else
				{
					if (i < last) { i++; }
					//se espera un tipo de dato para comparar
					if (((Token)(tokens[i])).No_token != 103 || ((Token)(tokens[i])).No_token != 104 || ((Token)(tokens[i])).No_token != 105 || ((Token)(tokens[i])).No_token != 106
					|| ((Token)(tokens[i])).No_token != 107 || ((Token)(tokens[i])).No_token != 138 || ((Token)(tokens[i])).No_token != 139 || ((Token)(tokens[i])).No_token != 153)
					{ Console.WriteLine("Error de sintaxis. Se esperaba un tipo de dato. "); }
					if (i < last) { i++; }
					if (((Token)(tokens[i])).No_token != 127) { Console.WriteLine("Error de sintaxis. Se esperaba '{' "); }
					if (i < last) { i++; }
					bloque_instrucciones();

					if (bloque_instrucciones() != true) { Console.WriteLine("Error de sintaxis."); }
					if (((Token)(tokens[i])).No_token != 145) { Console.WriteLine("Error de sintaxis. Se esperaba 'break' "); }
					if (i < last) { i++; }
					if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
					if (i < last) { i++; }
					if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
					if (i < last) { i++; }
					ban = false;
				}
			} while (ban==false);

			if (((Token)(tokens[i])).No_token != 128) { Console.WriteLine("Error de sintaxis. Se esperaba '}' "); }
			if (i < last) { i++; }
		}
		public void escribir() {
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }
			bool ban = true;
			do
			{
				if (((Token)(tokens[i])).No_token != 106 || ((Token)(tokens[i])).No_token != 101)
				{ Console.WriteLine("Error de sintaxis. Se esperaba string "); }
				//preguntamos si la variable existe 
				if (variables.Contains(((Token)(tokens[i])).Simbolo) != true) { Console.WriteLine("Error de semántica. La variable no ha sido declarada anteriormente."); }
				if (i < last) { i++; }
				if (((Token)(tokens[i])).No_token != 110) { i++; ban = false; } else { ban = true; }

			} while (ban==false);
			if (i < last) { i++; }

			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
			if (i < last) { i++; }
		}
		public void leer()
		{
			int last = tokens.Count - 1;//último token registrado

			if (((Token)(tokens[i])).No_token != 125) { Console.WriteLine("Error de sintaxis. Se esperaba '(' "); }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token != 126) { Console.WriteLine("Error de sintaxis. Se esperaba ')' "); }
			if (i < last) { i++; }
			if (((Token)(tokens[i])).No_token != 109) { Console.WriteLine("Error de sintaxis. Se esperaba ';' "); }
			if (i < last) { i++; }
		}

		public bool bloque_instrucciones()
		{
			int last = tokens.Count - 1;//último token registrado

			bool ban = true;
			do
			{
				if (((Token)(tokens[i])).No_token != 101 || ((Token)(tokens[i])).No_token != 141 || ((Token)(tokens[i])).No_token != 142 || ((Token)(tokens[i])).No_token != 143
				|| ((Token)(tokens[i])).No_token != 144 || ((Token)(tokens[i])).No_token != 146 || ((Token)(tokens[i])).No_token != 148 || ((Token)(tokens[i])).No_token != 149)
				{ ban = true; }
				else
				{
					instrucciones(((Token)(tokens[i])).No_token);
					if (i < last) { i++; }
					ban = false;

				}

			} while (ban == false);

			return ban;
		}

	
		public void Errores(int error)
		{
			switch (error)
			{
				case -1: { Console.WriteLine("Error léxico, debe finalizar con números."); break; }
				case -2: { Console.WriteLine("Error léxico, debe continuar  con números después del punto."); break; }
				case -3: { Console.WriteLine("Error léxico, símbolo indefinido. Para operador AND escribir &&."); break; }
				case -4: { Console.WriteLine("Error léxico, símbolo indefinido. Para operador OR escribir ||."); break; }
				case -5: { Console.WriteLine("Error léxico, símbolo indefinido."); break; }
				case -6: { Console.WriteLine("Error léxico, no se cerraron comillas."); break; }
				case -7: { Console.WriteLine("Error léxico, hay más de un carácter."); break; }
				case -8: { Console.WriteLine("Error léxico, no se cerró con apóstrofe."); break; }
			}

		}
		public int Reserve(string simb)
		{
			int token = 0;
			switch (simb)
			{
				case "main": { token= 133; break; }
				case "int": { token = 134; break; }
				case "string": { token = 135; break; }
				case "char": { token = 136; break; }
				case "double": { token = 137; break; }
				case "true": { token = 138; break; }
				case "false": { token = 139; break; }
				case "else": { token = 140; break; }
				case "if": { token = 141; break; }
				case "for": { token = 142; break; }
				case "while": { token = 143; break; }
				case "switch": { token = 144; break; }
				case "break": { token = 145; break; }
				case "do": { token = 146; break; }
				case "null": { token = 147; break; }
				case "write": { token = 148; break; }
				case "read": { token = 149; break; }
				case "namespace": { token = 150; break; }
				case "bool": { token = 151; break; }
				case "case": { token = 152; break; }
				case "default": { token = 153; break; } 
			}

			return token;
		} 

    } //fin de la clase


} //fin del namespace