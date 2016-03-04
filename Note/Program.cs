
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
	class MainClass
	{
		
		//
		// ─── CONST STUFF ────────────────────────────────────────────────────────────────────────────────────
		//
		
			static readonly string note_file_address = "/users/pmk/Dropbox (personal)/.KaryAppData/notes.txt";
			static readonly string astrisk_with_space = " ✣ ";

		//
		// ─── COMPUTE MAX ROMAN NUMBER SIZE IN RANGE ─────────────────────────────────────────────────────────
		//

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

		//
		// ─── PRINT FOOTER ───────────────────────────────────────────────────────────────────────────────────
		//

			public static void print_footer ( int max_size ) {
				Terminal.PrintLn( 
					Utilities.Repeat( " " , max_size + 8 ) + "─────────────── ✣ ✣ ✣ ───────────────"
				);
			}

		//
		// ─── PRINT HEADER ───────────────────────────────────────────────────────────────────────────────────
		//

			// 
			// THIS FUNCTION GENERATES THIS HEADER TEXT IN THE
			// TOP OF THE APPLICATION
			//
			//                 ⎧           ⎫            
            //     ───────── ✣ ⎨   notes   ⎬ ✣ ─────────
			//                 ⎩           ⎭            
			//

			public static void print_header ( int max_size ) {
				
				// the "─────────" lines in start and end of the header
				string header_line = Utilities.Repeat( "─" , 9 );
				
				Terminal.PrintLn(
					
					// adds spacing to the start of the text to match the other boxes
					Utilities.Concatenate(
						Utilities.Repeat( " " , max_size + 8 ),
						
						// adds an astrisk + line to the end of the result
						Utilities.Concatenate( 
							
							// adds the line + astrisk + note box together
							Utilities.Concatenate( 
								header_line + astrisk_with_space, 
								
								// This Generates the curly brackted box with
								// note text on it.
								TextShapes.ShapeWithOption( 
									"notes", 
									11 , 0 , 0 , 
									TextJustification.Center , 
									TextShapeFormat.CurlyBracket
								) 
							), 
							
							astrisk_with_space + header_line 
						)
					)			
				);
				
				Terminal.NewLine();
			}
			
		//
		// ─── LOAD NOTE ──────────────────────────────────────────────────────────────────────────────────────
		//

			public static string LoadNoteString ( ) {
				string noteString = "";
				try {
					using ( var reader = new StreamReader( note_file_address ) ) {
						noteString = reader.ReadToEnd( );
					}
				} catch {
					Terminal.PrintLn( "Loading the note failed" );
					Environment.Exit( 1 );
				}
				return noteString;
			}
			
		//
		// ─── LOAD NOTE STRINGS ──────────────────────────────────────────────────────────────────────────────
		//
		
			public static string[ ] LoadNotes ( ) {
				return LoadNoteString( ).Split( '\n' );
			}

		//
		// ─── PRINT THE NOTE ─────────────────────────────────────────────────────────────────────────────────
		//

			public static void PrintNote ( ) {
				try {
					using ( var reader = new StreamReader ( note_file_address ) ) {
						
						// Defs
						var lines 	= reader.ReadToEnd ().Split ('\n');
						var size 	= max_roman_size(lines.Length);
						int index	= 0;
						
						// Body
						Terminal.PrintLn ();
						print_header( size );
						foreach ( var element in lines ) {
							if ( element != "" ) {
								index++;

								// Creating the note box
								string roman_string = Numerics.Roman( index );
								string roman_number = Utilities.Repeat( " " , size - roman_string.Length + 1 ) + roman_string + astrisk_with_space;
								string note = TextShapes.Box( element , 41 , 1 , 0 , TextJustification.Left );
								note = Utilities.Concatenate( roman_number , note ) ;
								
								Terminal.PrintLn (note);								
							}
						}
						Terminal.NewLine();
						print_footer( size );
						Terminal.PrintLn ();
					}
				} catch {
					Terminal.PrintLn ( "Loading the note file stream failed." );
				}
			}
			
		//
		// ─── EDIT NOTE AT INDEX ─────────────────────────────────────────────────────────────────────────────
		//
			
			public static void EditNoteAtIndex ( string indexString ) {
				try {
					int index = int.Parse( indexString );
					string[ ] notes = LoadNotes( );
					if ( index > -1 && index < notes.Length ) {
						// Getting and updating the new note
						notes[ index ] = GetNoteFromInterface( notes[ index ] );
						// Storing the notes
						StoreNoteList( notes );
					} else {
						Terminal.PrintLn( "No note with this index found." );
					}
				} catch {
					Terminal.PrintLn( "Index number in not a good shape." );
				}
			}
		
		//
		// ─── STORE NOTE LIST ────────────────────────────────────────────────────────────────────────────────
		//
			
			public static void StoreNoteList ( string[ ] noteList ) {
				try {
					using( StreamWriter writer = new StreamWriter ( note_file_address ) ) {
						foreach ( string note in noteList ) {
							writer.WriteLine( note );
						}
					}
				} catch {
					Terminal.PrintLn( "Could not store the notes..." );
				}
			}
			
		//
		// ─── PRINT THE HELP PAGE ────────────────────────────────────────────────────────────────────────────
		//
		
			public static void PrintHelpPage ( ) {
				Terminal.PrintLn(																	);
				Terminal.PrintLn( "  Kary Note - Copyright 2015 Pouya Kary <k@arendelle.org>" 		);
				Terminal.PrintLn( "  ───────────────────────────────────────────────────────────"	);
				Terminal.PrintLn( "    % note              Prints the notes on the Terminal." 		);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note add          Open's a prompt for you to add notes"	);
				Terminal.PrintLn( "                        with special characters you can't add"	);
				Terminal.PrintLn( "                        in normal mode."							);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note [note]       Let's you append a note." 				);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note rm [index]   Removes the note at [index]." 			);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "  This is a tiny software released under GNU GPL 3. To get" 		);
				Terminal.PrintLn( "  more information you may consult the webpage at:"				);
				Terminal.PrintLn(																	);
				Terminal.PrintLn( "    - http://github.com/pmkary/note"								);
				Terminal.PrintLn(																	);
			}
		
		//
		// ─── REMOVE NOTE ────────────────────────────────────────────────────────────────────────────────────
		//
		
			public static void RemoveNotes ( string[ ] args ) {
				
				//
				// - - loading the file - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//

						string[ ] string_parts = LoadNotes( );

				//
				// - - removing the indexes - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//

						//
						// WRITING THE INDEXES WITHOUT WITING THE INDEX WE'RE GOING
						// TO REMOVE. PRETTY COOL WAY HA?
						// 

						int index = 0;
						using ( var writer = new StreamWriter( note_file_address ) ) {
							foreach ( var element in string_parts ) {
								if ( element != "" ) {
									index++;
									bool is_not_to_be_removed = true;
									foreach ( string arg in args ) {
										if ( index.ToString( ) == arg ) {
											is_not_to_be_removed = false;
										}
									}
									if ( is_not_to_be_removed ) {
										writer.WriteLine( element );
									}
								}
							}
						}

				//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				
			}
			
		//
		// ─── NEW NOTE INTERFACE ─────────────────────────────────────────────────────────────────────────────
		//
		
			public static string GetNoteFromInterface ( string load_note_with_text = "" ) {
				
				//
				// - - main box - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//
				
					Terminal.PrintLn(
						Kary.Text.TextShapes.Box( "NEW NOTE" , Terminal.Width - 6 , 1 , 0 , TextJustification.Left )
					);

				//
				// - - extra box lines - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//
				
					Terminal.X = 11; Terminal.Y -= 3;
					Terminal.Print( '┬' );
					
					Terminal.X--; Terminal.Y++;
					Terminal.Print( '│' );
					
					Terminal.X--; Terminal.Y++;
					Terminal.Print( '┴' );

				//
				// - - reading the input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//
				
					Terminal.Y --;
					Terminal.X = 13;
					string new_note = Terminal.TextBox( Terminal.Width - 17 , load_note_with_text );
					Terminal.Y++;
				
				//
				// - - and done - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//

					return new_note;
					
					
				//  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				
			}
			
		//
		// ─── NEW NOTE SAVER ─────────────────────────────────────────────────────────────────────────────────
		//
		
			public static void StoreNewNote ( string new_note_text ) {
				try {
					using ( var reader = new StreamReader ( note_file_address ) ) {
						string note_file_string = reader.ReadToEnd( );
						reader.Close( );
						try {
							using ( var writer = new StreamWriter ( note_file_address ) ) {
								writer.Write ( note_file_string + "\n" + new_note_text );
							}
						} catch  {
							Terminal.PrintLn ( "Writing to the note stream failed." );
						}
					}
				} catch {
					Terminal.PrintLn ( "Loading the note file stream failed." );
				}
			}

		//
		// ─── GET NEW NOTE FROM ARGS ─────────────────────────────────────────────────────────────────────────
		//
		
			public static string GetNewNoteFromArgs ( string[ ] args ) {
				string note_text = "";
				foreach ( var arg in args ) {
					note_text += arg + " ";
				}
				return note_text;
			}

		//
		// ─── MAIN ───────────────────────────────────────────────────────────────────────────────────────────
		//

			public static void Main ( string[ ] args ) {

				if ( args.Length == 0 ) {

					PrintNote( );

				} else if ( args.Length >= 1 ) {
					
					if ( args[ 0 ] == "help" ) {
						
						PrintHelpPage( );
						
					} else if ( args[ 0 ] == "rm" || args[ 0 ] == "remove" ) {
						
						RemoveNotes( args );

					} else if ( args[ 0 ] == "add" || args[ 0 ] == "new" ) {

						StoreNewNote( GetNoteFromInterface( ) );
						
					} else if ( args.Length == 2 && args[ 0 ] == "edit" ) {
						
						EditNoteAtIndex( args[ 1 ] );
						
					} else {
						
						StoreNewNote( GetNewNoteFromArgs( args ) );
						
					}
				}
			}
		
		// ────────────────────────────────────────────────────────────────────────────────────────────────────
			
	}
}
