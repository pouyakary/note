
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
		
			static string note_file_address;
			static readonly string note_file_address_config_file = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Personal ), ".notepath" );
			static readonly string astrisk_with_space = " ✣ ";
			
			static readonly int _column_size = 41;
			static readonly int _left_margin = 3;
			
		//
		// ─── CONFIG FILE LOADER ─────────────────────────────────────────────────────────────────────────────
		//
		
			public static bool LoadPathAddress ( ) {
				try {
					using( StreamReader reader = new StreamReader( note_file_address_config_file ) ) {
						note_file_address = reader.ReadToEnd( ).Trim( );
					}
					return true;
				} catch {
					Report( "Could not load the config file." );
					return false;
				}
			}

		//
		// ─── PRINT FOOTER ───────────────────────────────────────────────────────────────────────────────────
		//

			public static void print_footer ( ) {
				Terminal.PrintLn( 
					Utilities.Repeat( " " , _left_margin + 4 ) + "─────────────── ✣ ✣ ✣ ───────────────"
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

			public static void print_header ( ) {
				
				// the "─────────" lines in start and end of the header
				string header_line = Utilities.Repeat( "─" , 9 );
				
				Terminal.PrintLn(
					
					// adds spacing to the start of the text to match the other boxes
					Utilities.Concatenate(
						Utilities.Repeat( " " , _left_margin + 4 ),
						
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
					Report( "Loading the note failed" );
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
		// ─── GENERATE NOTE BOX ──────────────────────────────────────────────────────────────────────────────
		//
		
			public static string GenerateNoteBox ( string note , int index ) {
				
				// init commands 
				string index_string = Kary.Text.Numerics.Roman( index );
				string replacer_placeholder_string = Utilities.Repeat( "%" , index_string.Length + 3 );
				int replacer_placeholder_string_lenght = replacer_placeholder_string.Length;
				
				// adding the replacer_placeholder_string
				string note_string = replacer_placeholder_string + note;
				note_string = Justify.Left( note_string , _column_size );
				
				// checking if there is a second line
				string[ ] lines = note_string.Split( '\n' );
				if ( lines.Length != 1 ) {
					// adjusting
					lines[ 1 ] = replacer_placeholder_string + lines[ 1 ];
					note_string = string.Join( " " , lines );
				} else {
					note_string.Replace( replacer_placeholder_string , index_string + "│ " );
				}
				
				note_string = Kary.Text.TextShapes.Box( note_string , _column_size , 1 , 0 , TextJustification.Left );
				lines = note_string.Split( '\n' );
				
				// adjusting the boom
				string bottom_line = lines[ 2 ];
				if ( lines.Length == 3 ) {
					bottom_line = Utilities.RemoveFromStart( lines[ 2 ] , "└" + Utilities.Repeat( "─" , replacer_placeholder_string_lenght ) );
					bottom_line = "└" + Utilities.Repeat( "─" , replacer_placeholder_string_lenght - 1 ) + "┴" + bottom_line;
				} else {
					bottom_line = Utilities.RemoveFromStart( lines[ 2 ] , "│ " + replacer_placeholder_string );
					bottom_line = "├" + Utilities.Repeat( "─" , replacer_placeholder_string_lenght - 1 ) + "┘ " + bottom_line;
				}
				
				lines[ 2 ] = bottom_line;
				
				// fix first line
				lines[ 0 ] = Utilities.RemoveFromStart( lines[ 0 ] , "┌" + Utilities.Repeat( "─" , replacer_placeholder_string_lenght ) );
				lines[ 0 ] = "┌" + Utilities.Repeat( "─" , replacer_placeholder_string_lenght - 1 ) + "┬" + lines[ 0 ];
				
				// fix substitution
				lines[ 1 ] = lines[ 1 ].Replace( replacer_placeholder_string , index_string + " │ " );

				// finishing
				note_string = string.Join( "\n" , lines );
				note_string = Utilities.Concatenate( "   " , note_string );
				
				return note_string;
			}
			
		//
		// ─── PRINT THE NOTE ─────────────────────────────────────────────────────────────────────────────────
		//

			public static void PrintNote ( ) {
				try {
					using ( var reader = new StreamReader ( note_file_address ) ) {
						
						// Defs
						var lines 	= reader.ReadToEnd ().Split ('\n');
						int index	= 0;
						
						// Body
						Terminal.PrintLn ();
						print_header( );
						foreach ( var element in lines ) {
							if ( element != "" ) {
								index++;
								
								string note = GenerateNoteBox( element , index );
								Terminal.PrintLn (note);								
							}
						}
						Terminal.NewLine();
						print_footer( );
						Terminal.PrintLn ();
					}
				} catch {
					Report( "Loading the note file stream failed." );
				}
			}
			
		//
		// ─── EDIT NOTE AT INDEX ─────────────────────────────────────────────────────────────────────────────
		//
			
			public static void EditNoteAtIndex ( string indexString ) {
				try {
					int index = int.Parse( indexString ) - 1;
					string[ ] notes = LoadNotes( );
					if ( index > -1 && index < notes.Length ) {
						// Getting and updating the new note
						notes[ index ] = GetNoteFromInterface( notes[ index ] );
						// Storing the notes
						StoreNoteList( notes );
					} else {
						Report( "No note with this index found." );
					}
				} catch {
					Report( "Index number in not a good shape." );
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
					Report( "Could not store the notes..." );
				}
			}
			
		//
		// ─── PRINT THE HELP PAGE ────────────────────────────────────────────────────────────────────────────
		//
		
			public static void PrintHelpPage ( ) {
				Terminal.PrintLn(																	);
				Terminal.PrintLn( "  Kary Note - Copyright 2015-2016 Pouya Kary <kary@gnu.org>"		);
				Terminal.PrintLn( "  ───────────────────────────────────────────────────────────"	);
				Terminal.PrintLn( "    % note                Prints the notes on the Terminal." 	);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note add            Open's a prompt for you to add notes"	);
				Terminal.PrintLn( "                          with special characters you can't add"	);
				Terminal.PrintLn( "                          in normal mode."						);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note [note]         Let's you append a note." 				);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note rm [index]     Removes the note at [index]." 			);
				Terminal.NewLine(																	);
				Terminal.PrintLn( "    % note edit [index]   Lets you edit note at [index]." 		);
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
						Kary.Text.TextShapes.Box( "NOTE" , Terminal.Width - 5 , 1 , 0 , TextJustification.Left )
					);

				//
				// - - extra box lines - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//
				
					Terminal.X = 7; Terminal.Y -= 3;
					Terminal.Print( '┬' );
					
					Terminal.X--; Terminal.Y++;
					Terminal.Print( '│' );
					
					Terminal.X--; Terminal.Y++;
					Terminal.Print( '┴' );

				//
				// - - reading the input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
				//
				
					Terminal.Y --;
					Terminal.X = 9;
					string new_note = Terminal.TextBox( Terminal.Width - 12 , load_note_with_text );
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
							Report ( "Writing to the note stream failed." );
						}
					}
				} catch {
					Report ( "Loading the note file stream failed." );
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
		// ─── EDIT COMMAND NO ARG ERROR ──────────────────────────────────────────────────
		//	
			
			public static void PrintEditNoArgError ( ) {
				int countOfNotes = LoadNotes( ).Length;
				Random rnd = new Random( );
				Report ( 
					"Edit command takes one argument to know which note you'd like to edit." +
					" for now you have " + countOfNotes + " notes. So you can do something like:\n" +
					"   % note edit " + rnd.Next( countOfNotes ).ToString( )
				);
			}
			
		//
		// ─── NO RM ARGS ERROR ───────────────────────────────────────────────────────────
		//
			
			public static void PrintRemoveNoArgError ( ) {
				int countOfNotes = LoadNotes( ).Length;
				Random rnd = new Random( );
				Report ( 
					"Remove command takes one or more argument to know which note you'd like to remove." +
					" for now you have " + countOfNotes + " notes. So you can do something like:\n" +
					"   % note rm " + rnd.Next( countOfNotes ).ToString( ) + "\n" + 
					"Or you may remove more than one note like:\n" + 
					"   % note rm " + rnd.Next( countOfNotes ).ToString( ) + " " + 
					rnd.Next( countOfNotes ).ToString( ) + " " + rnd.Next( countOfNotes ).ToString( )
				);
			}
			
		//
		// ─── ERROR REPORTER ─────────────────────────────────────────────────────────────
		//
			
			public static void Report ( string message ) {
				Terminal.NewLine();
				Terminal.Red( );
				Terminal.PrintLn( "  O P E R A T I O N   F A I L I U R E\n" );
				Terminal.Reset( );
				string msg = Justify.Left( message , 50 );
				msg = Utilities.Perpend( msg , "  │ " );
				Terminal.PrintLn( msg );
				Terminal.NewLine( ); 
			}

		//
		// ─── MAIN ───────────────────────────────────────────────────────────────────────────────────────────
		//

			public static void Main ( string[ ] args ) {
				
				//
				// LOADING CONFIG FILE
				//
				
				if ( !LoadPathAddress( ) ) {
					return;
				}
				
				
				//
				// PARSING THE ARGUMENTS
				//

				if ( args.Length == 0 ) {

					PrintNote( );

				} else if ( args.Length >= 1 ) {
					
					if ( args[ 0 ] == "help" ) {
						
						PrintHelpPage( );
						
					} else if ( args[ 0 ] == "edit" && args.Length == 1 ) {
						
						PrintEditNoArgError( );
						
					} else if ( args[ 0 ] == "rm" || args[ 0 ] == "remove" ) {
						
						if ( args.Length == 1 ) {
							PrintRemoveNoArgError( );
						} else {
							RemoveNotes( args );
						}
						
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
