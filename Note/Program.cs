
//
//  Program.cs
//
//  Created by Pouya Kary on 2015/4/13
//  Copyright (c) 2015 Pouya Kary. All rights reserved.
//


using System;
using System.IO;
using Kary.Text;
using Kary;

namespace Note
{

	/* ─────────────────────────────────────────────────────────── *
	 * :::::::::::::::::::: K A R Y   N O T E :::::::::::::::::::: *
	 * ─────────────────────────────────────────────────────────── */

	class MainClass
	{
		static string note_file_address = "/users/pmk/Dropbox (personal)/.KaryAppData/notes.txt";


		public static int max_roman_size ( int max_number ) {

			int max_size = 0;

			for ( int index = 0 ; index < max_number ; index++ ) {

				string roman_string = Numerics.Roman( index );

				if ( roman_string.Length > max_size ) {

					max_size = roman_string.Length;

				}

			}

			return max_size;

		}


		public static void Main (string[] args)
		{
			//
			// PRINTS THE NOTE
			//

			if (args.Length == 0) {

				try {
					using (var reader = new StreamReader (note_file_address)) {
					
						var lines = reader.ReadToEnd ().Split ('\n');

						var size = max_roman_size(lines.Length);


						int i = 0;

						Terminal.PrintLn ();



						foreach (var index in lines) {

							if (index != "") {

								i++;

								//
								// SOME PANTHER FORMATTING, NOT REALLY A
								// POWER USER ONE!
								//

								string roman_string = Numerics.Roman( i );

								string roman_number = Utilities.Repeat( " " , size - roman_string.Length + 1 ) + roman_string + " ✤ ";

								string note = TextShapes.Box( index , 40 , 1 , 0 , TextJustification.Left );

								note = Utilities.Concatenate( roman_number , note ) ;

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

			} else if (args.Length >= 1) {

				if ( args[ 0 ] == "help" ) {

					Terminal.PrintLn( );
					Terminal.PrintLn( "  Kary Note - Copyright 2015 Pouya Kary <k@arendelle.org>" );
					Terminal.PrintLn( "  ───────────────────────────────────────────────────────────" );
					Terminal.PrintLn( "    % note              Prints the notes on the Terminal." );
					Terminal.NewLine();
					Terminal.PrintLn( "    % note add          Open's a prompt for you to add notes");
					Terminal.PrintLn( "                        with special characters you can't add");
					Terminal.PrintLn( "                        in normal mode.");
					Terminal.NewLine();
					Terminal.PrintLn( "    % note [note]       Let's you append a note." );
					Terminal.NewLine();
					Terminal.PrintLn( "    % note rm [index]   Removes the note at [index]." );
					Terminal.NewLine();
					Terminal.PrintLn( "  This is a tiny software released under GNU GPL 3. To get" );
					Terminal.PrintLn( "  more information you may consult the webpage at:" );
					Terminal.PrintLn( );
					Terminal.PrintLn( "    - http://github.com/pmkary/note" );
					Terminal.PrintLn( );
				
				} else if ( args[ 0 ] == "rm" || args[ 0 ] == "remove" ) {
				
					string full_string = "";

					try {
						using ( var reader = new StreamReader( note_file_address ) ) {
				
							full_string = reader.ReadToEnd( );

						}
					} catch {
						Terminal.PrintLn( "Loading the note failed" );
						Environment.Exit( 1 );
					}

					var string_parts = full_string.Split( '\n' );


					//
					// WRITING THE INDEXES WITHOUT
					// WITING THE INDEX WE'RE GOING
					// TO REMOVE. PRETTY COOL WAY HA?
					//

					int i = 0;

					using ( var writer = new StreamWriter( note_file_address ) ) {

						foreach ( var index in string_parts ) {

							if ( index != "" ) {

								i++;

								bool is_not_to_be_removed = true;

								foreach ( var arg in args ) {

									if ( i.ToString( ) == arg ) {

										is_not_to_be_removed = false;

									}
								}


								if ( is_not_to_be_removed ) {

									writer.WriteLine( index );

								}
							}
						}
					}

					//
					// AND WE'RE ALL DONE
					//





				
				//
				// ADD NOTE
				//

				} else if ( args[ 0 ] == "add" || args[ 0 ] == "new" ) {

					//
					// INTERFACE
					//

					// the main box

					Terminal.PrintLn(
					
						Kary.Text.TextShapes.Box( "NEW NOTE" , Terminal.Width - 6 , 1 , 0 , TextJustification.Left )
					
					);


					// the extra box lines

					Terminal.X = 11;

					Terminal.Y -= 3;

					Terminal.Print( '┬' );

					Terminal.X--;

					Terminal.Y++;

					Terminal.Print( '│' );

					Terminal.X--;

					Terminal.Y++;

					Terminal.Print( '┴' );


					// making the input

					Terminal.Y --;

					Terminal.X = 13;

					string new_note = Terminal.TextBox( Terminal.Width - 17 );

					Terminal.Y++;



					//
					// ADDING THE NOTES
					//

					try {
						using (var reader = new StreamReader ( note_file_address ) ) {

							string note_file_string = reader.ReadToEnd ();

							reader.Close ();

							try {

								using (var writer = new StreamWriter ( note_file_address ) ) {

									writer.Write (note_file_string + "\n" + new_note);

								}

							} catch  {
								Terminal.PrintLn ("Writing to the note stream failed.");
							}
						}

					} catch{
						Terminal.PrintLn ("Loading the note file stream failed.");
					}







				//
				// NEW NOTE
				//

				} else {

					try {
						using (var reader = new StreamReader(note_file_address)) {

							string note_file_string = reader.ReadToEnd ();

							reader.Close ();

							try {

								using (var writer = new StreamWriter (note_file_address)) {

									string new_note = "";

									foreach ( var arg in args ) {

										new_note += arg + " ";

									}

									writer.Write (note_file_string + "\n" + new_note);
								}

							} catch  {
								Terminal.PrintLn ("Writing to the note stream failed.");
							}
						}
					} catch{
						Terminal.PrintLn ("Loading the note file stream failed.");
					}

				}
			}
		}
	}
}

//
// DONE
//
