
//
//  Program.cs
//
//  Created by Pouya Kary on 2015/4/13
//  Copyright (c) 2015 Pouya Kary. All rights reserved.
//


using System;
using System.IO;
using Panther;
using Panther.Text;

namespace Note
{

	/* ─────────────────────────────────────────────────────────── *
	 * :::::::::::::::::::: K A R Y   N O T E :::::::::::::::::::: *
	 * ─────────────────────────────────────────────────────────── */

	class MainClass
	{
		static string note_file_address = "/Users/pmk/Dropbox/.notes";

		public static void Main (string[] args)
		{
			//
			// PRINTS THE NOTE
			//

			if (args.Length == 0) {

				try {

					using (var reader = new StreamReader (note_file_address)) {
					
						var lines = reader.ReadToEnd ().Split ('\n');
						int i = 0;

						Terminal.PrintLn ();

						foreach ( var index in lines ) {

							if (index != "") {

								i++;

								//
								// SOME PANTHER FORMATTING, NOT REALLY A
								// POWER USER ONE!
								//

								string roman_number = Numerics.Roman (i);
								string note = Justify.Left (index, 40);
								string spacing = Utilities.Repeat (" ",roman_number.Length + 3);
								note = Utilities.Perpend (note, spacing + " ");
								note = Utilities.RemoveFromStart (note, spacing);
								note = " " + roman_number.ToString () + " →" + note;

								Terminal.PrintLn (note);
								Terminal.PrintLn ();
							}
						}
					}
						
				} catch {

					Terminal.PrintLn ("Loading the note file stream failed.");

				}

			//
			// COMMANDS WITH ONE ARG
			//

			} else if (args.Length == 1) {

				if (args[0] == "add") {
				
					try {
						using (var reader = new StreamReader(note_file_address)) {

							string note_file_string = reader.ReadToEnd ();

							reader.Close ();

							try {

								using (var writer = new StreamWriter (note_file_address)) {
								
									note_file_string += "\n" + Terminal.Input ();
									writer.Write (note_file_string);

								}
									
							} catch  {
								Terminal.PrintLn ("Writing to the note stream failed.");
							}
						}
					} catch{
						Terminal.PrintLn ("Loading the note file stream failed.");
					}
						
				
				} else if (args[0] == "reset" ) {

					try {
						using (var writer = new StreamWriter (note_file_address)) {
							writer.Write ("");
						}
					} catch {
						Terminal.PrintLn ("Writing to the note stream failed.");
					}


				} else if (args[0] == "help") {

					Terminal.PrintLn ();
					Terminal.PrintLn ("  Kary Note - Copyright 2015 Pouya Kary <k@arendelle.org>");
					Terminal.PrintLn ("  ───────────────────────────────────────────────────────────");
					Terminal.PrintLn ("   % note              Prints the notes on the Terminal");
					Terminal.PrintLn ("   % note add          Let's you append a note");
					Terminal.PrintLn ("   % note reset        Removes all the notes");
					Terminal.PrintLn ("   % note rm [index]   Removes the note at [index]");
					Terminal.PrintLn ();
					Terminal.PrintLn ("  This is a tiny software released under GNU GPL 3. To get");
					Terminal.PrintLn ("  more information you may consult the webpage at:");
					Terminal.PrintLn ();
					Terminal.PrintLn ("  - http://github.com/pmkary/note");
					Terminal.PrintLn ();
				}
 
			} else if (args.Length == 2) {

				if (args[0] == "rm") {
				
					//
					// PARSING THE NUMBER TO BE REMOVED
					//

					int rm_arg = 0;

					try {

						rm_arg = int.Parse (args[1]);

					} catch  {
						Terminal.PrintLn ("Bad note rm number. It must be a simple integer");
					}


					//
					// TAKING CARE OF THE REST
					//

					string new_string = "";
					string full_string = "";

					try {
						using (var reader = new StreamReader (note_file_address)) {
				
							full_string = reader.ReadToEnd ();

						}
					} catch {
						Terminal.PrintLn ("Loading the note failed");
						Environment.Exit (1);
					}

					var string_parts = full_string.Split ('\n');



					if (string_parts.Length <= rm_arg || rm_arg < 0 ) {

						Terminal.PrintLn ("Out of range index");
						Environment.Exit (2);

					}



					int i = 0;

					using (var writer = new StreamWriter (note_file_address)) {

						foreach (var index in string_parts) {

							if (index != "") {

								i++;
								if (i != rm_arg) {

									writer.WriteLine (index);

								}
							}
						}
					}

					//
					// AND WE'RE ALL DONE
					//
				}
			}
		}
	}
}
