//
//  Program.cs
//
//  Created by Pouya Kary on 2015/4/13
//  Copyright (c) 2015 Pouya Kary. All rights reserved.
//

//
// ─── IMPORTS ────────────────────────────────────────────────────────────────────
//


	using System;
	using System.IO;
	using System.Collections.Generic;
	using Kary.Text;
	using Kary;

//
// ─── THE WHOLE SOFTWARE ─────────────────────────────────────────────────────────
//

	namespace Note {
		class MainClass {

			//
			// ─── CONSTANTS ───────────────────────────────────────────────────
			//

				static string note_file_address;
				static readonly string note_file_address_config_file =
                    Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Personal ), ".kary-note-path" );
				static readonly string asterisk_with_space =
                    " ✣ ";
				static readonly int _column_size =
                    ( Terminal.Width - 17 ) / 2;
				static readonly int _left_margin =
                    3;

			//
			// ─── CONFIG FILE LOADER ──────────────────────────────────────────
			//

				public static bool LoadPathAddress ( ) {
					try {
						using( StreamReader reader = new StreamReader( note_file_address_config_file ) ) {
							note_file_address = reader.ReadToEnd( ).Trim( );
						}
						return true;
					} catch {
						Report( "Could not load the config file.");
						return false;
					}
				}

			//
			// ─── PRINT HEADER ────────────────────────────────────────────────
			//

				public static void print_header ( ) {
                    //Terminal.NewLine( );
                    // string header =
                    //     "  " + Kary.Text.Utilities.Repeat( "─", Terminal.Width - 4 ) +
                    //     "\n  N O T E S";
                    // Terminal.PrintLn( header );

					Terminal.NewLine( );
				}

			//
			// ─── PRINT FOOTER ────────────────────────────────────────────────
			//

				public static void print_footer ( ) {
					Terminal.NewLine();
					Terminal.Y--;
					//Terminal.PrintLn( Utilities.Repeat( "─" , Terminal.Width - 12 ) + " © 2016 ────" );
				}

			//
			// ─── LOAD NOTE ───────────────────────────────────────────────────
			//

				public static string LoadNoteString ( ) {
					string noteString = "";
					try {
						using ( var reader = new StreamReader( note_file_address ) ) {
							noteString = reader.ReadToEnd( );
						}
					} catch {
						Report("Loading the note failed");
						Environment.Exit( 1 );
					}
					return noteString;
				}

			//
			// ─── LOAD NOTE STRING ────────────────────────────────────────────
			//

				public static string[ ] LoadNotes ( ) {
					string[ ] lines =
                        LoadNoteString( ).Split( '\n' );

					List<string> notes = new List<string> ();
					foreach ( string line in lines )
						if ( !( line == null || line == "" ) )
							notes.Add( line );

					return notes.ToArray();
				}

			//
			// ─── GENERATE NOTE BOX ───────────────────────────────────────────
			//

				public static string GenerateNoteBox ( string note , int index, int columns ) {

                    // local stuff
                    int localMargin = _left_margin;
                    if ( columns == 1 )
                        localMargin = 1;

                    int localColumnSize = _column_size;
                    if ( columns == 1 )
                        localColumnSize =
                            Terminal.Width - ( localMargin * 2 ) - 4;


					// format note
					note = note.Replace("-->", "→").Replace("->", "→");

					// init commands
					string index_string =
                        index.ToString( );
					string replacer_placeholder_string =
                        Utilities.Repeat( "%" , index_string.Length + 3 );
					int replacer_placeholder_string_length =
                        replacer_placeholder_string.Length;

					// adding the replacer_placeholder_string
					string note_string =
                        replacer_placeholder_string + note;
					note_string =
                        Justify.Left( note_string , localColumnSize );

					// checking if there is a second line
					string[ ] lines =
                        note_string.Split( '\n' );

					if ( lines.Length != 1 ) {
						// adjusting
						lines[ 1 ] =
                            replacer_placeholder_string + lines[ 1 ];
						note_string =
                            string.Join( " " , lines );
					} else {
						note_string.Replace( replacer_placeholder_string , index_string + "│ " );
					}

					note_string =
                        Kary.Text.TextShapes.Box( note_string , localColumnSize , 1 , 0 , TextJustification.Left );
					lines =
                        note_string.Split( '\n' );

					// adjusting the boom
					string bottom_line =
                        lines[ 2 ];

					if ( lines.Length == 3 ) {
						bottom_line =
                            Utilities.RemoveFromStart( lines[ 2 ] , "└" + Utilities.Repeat( "─" , replacer_placeholder_string_length ) );
						bottom_line =
                            "└" + Utilities.Repeat( "─" , replacer_placeholder_string_length - 1 ) + "┴" + bottom_line;
					} else {
						bottom_line =
                            Utilities.RemoveFromStart( lines[ 2 ] , "│ " + replacer_placeholder_string );
						bottom_line =
                            "├" + Utilities.Repeat( "─" , replacer_placeholder_string_length - 1 ) + "┘ " + bottom_line;
					}

					lines[ 2 ] = bottom_line;

					// fix first line
					lines[ 0 ] =
                        Utilities.RemoveFromStart( lines[ 0 ] , "┌" + Utilities.Repeat( "─" , replacer_placeholder_string_length ) );
					lines[ 0 ] =
                        "┌" + Utilities.Repeat( "─" , replacer_placeholder_string_length - 1 ) + "┬" + lines[ 0 ];

					// fix substitution
					lines[ 1 ] =
                        lines[ 1 ].Replace( replacer_placeholder_string , index_string + " │ " );

					// finishing
					note_string =
                        string.Join( "\n" , lines );
					note_string =
                        Utilities.Concatenate(
                            Utilities.Repeat( " ", localMargin ) , note_string );

					return note_string;
				}

			//
			// ─── GENERATE BOX ARRAY ──────────────────────────────────────────
			//

				public static string[ ] GenerateBoxArray ( string[ ] notes ) {
					int index = 0;
					List<string> boxes = new List<string>( ) ;
					foreach ( var element in notes ) {
						if ( element != "" ) {
							index++;
							boxes.Add( GenerateNoteBox( element , index, 2 ) );
						}
					}
					return boxes.ToArray( );
				}

			//
			// ─── MAKE STRING COLUMN ──────────────────────────────────────────
			//

				public static string generateStringColumn ( string[ ] notes , int start , int end ) {
					string result = "";
					for ( int index = start; index < notes.Length && index < end ; index++ ) {
						result += notes[ index ] + "\n";
					}
					return result;
				}

			//
			// ─── GET COLUMN SPLIT LOCATION ───────────────────────────────────
			//

				public static int GetColumnSplitLocation ( string[ ] column_list ) {
					int size =
                        CountLines( string.Join( "\n" , column_list ) ) / 2 ;
					int split_index = 0;
					int total_length = 0;

					for ( int index = 0; index < column_list.Length ; index++ ) {
						if ( total_length > size ) {
							split_index = index;
							goto finish;
						} else {
							total_length += CountLines( column_list[ index ] ) + 1;
						}
					}

					finish:
					return split_index;
				}

			//
			// ─── COUNT LINES ─────────────────────────────────────────────────
			//

				public static int CountLines ( string text ) {
					int lines = 0;
					foreach ( char character in text )
						if ( character == '\n' )
							lines++;

					return lines;
				}

			//
			// ─── FIX COLUMN SIZE ─────────────────────────────────────────────
			//

				public static void FixColumnSizes ( ref string left , ref string right ) {
					int difference =
                        CountLines( left ) - CountLines( right );
					string append_string =
                        Utilities.Repeat( "\n" , Math.Abs( difference ) + 1 );

					if ( difference > 0 )
						right += append_string;
					else
						left += append_string;

					int h =
                        CountLines( left );
					left =
                        Utilities.PlaceAtBox( _column_size + 6 , h , left );
					right =
                        Utilities.PlaceAtBox( _column_size , h , right );
				}

			//
			// ─── COMPUTE COLUMN BOX HEIGHT VIA THE MAIN COLUMN SIZE ──────────
			//

				public static int ComputeColumnBoxHeightViaTheMainColumn ( string[ ] column_boxes , int start , int end ) {
					int column_height = 0;
					for ( int index = start; index < end && index < column_boxes.Length; index++ ) {
						column_height += CountLines( column_boxes[ index ] );
					}
					return column_height;
				}

			//
			// ─── ADJUST SPLIT LOCATION ───────────────────────────────────────
			//

				public static void AdjustSplitLocation ( string[ ] column_list , ref int split_location ) {
					string left_column =
                        generateStringColumn( column_list , 0 , split_location );
					string right_column =
                        generateStringColumn( column_list , split_location , column_list.Length );
					int difference =
                        CountLines( left_column ) - CountLines( right_column );
					int last_left_box_height =
                        CountLines( column_list[ split_location - 1 ] );

					if ( last_left_box_height < difference )
						split_location--;
				}

				public static string RandomNoNotesMessage( ) {
					string[ ] messages = {
						"With no notes, it's so boring down here...",
						"I'm not sure if it's good or bad... you have no notes...",
						"My job as a note app is at risk with no notes...",
						"I have to feed a family, please land me some notes",
						"Economy has changed, people don't give a damn about notes",
						"You must have a good memory to have no notes",
						"No notes? Jeez what a mess..."
					};
					Random rnd = new Random( );
					return messages[ rnd.Next( messages.Length ) ];
				}

			//
			// ─── PRINT NOTES ─────────────────────────────────────────────────
			//

				public static void printNoteColumn ( string[ ] notes ) {

					if ( notes.Length == 0 ) {

						Terminal.PrintLn(
							Utilities.Perpend(
								TextShapes.Box( RandomNoNotesMessage( ).ToUpper( ) , Terminal.Width - 12, 2, 0, TextJustification.Center ) + "\n",
								"   "
							)
						);

					} else if ( notes.Length == 1 ) {

						Terminal.PrintLn( GenerateNoteBox( notes[ 0 ] , 1, 1 ) );
						Terminal.NewLine( );

					} else {

						string[ ] column_list =
                            GenerateBoxArray( notes );
						int split_location =
                            GetColumnSplitLocation( column_list );

						AdjustSplitLocation( column_list , ref split_location );

						string left_column =
                            generateStringColumn( column_list , 0 , split_location );
						string right_column =
                            generateStringColumn( column_list , split_location , column_list.Length );
						FixColumnSizes( ref left_column , ref right_column );

						string layout =
                            Utilities.Concatenate ( left_column , right_column );

						Terminal.Print( layout );

					}
				}

			//
			// ─── PRINT THE NOTE ──────────────────────────────────────────────
			//

				public static void PrintNote ( ) {
					try {
						// Defs
						var lines = LoadNotes();

						// Body
						print_header( );

                        if ( Terminal.Width > 50 ) {
						    printNoteColumn( lines );
                        } else {
                            PrintOneColumnNotes( lines );
                        }

						print_footer( );
					} catch ( Exception e ) {
						Report( "Loading the note file stream failed.");
					}
				}

            //
            // ─── PRINT ONE COLUMN NOTES ──────────────────────────────────────
            //

                private static void PrintOneColumnNotes ( string[ ] notes ) {
                    int index = 0;
                    foreach ( string note in notes )
                        Terminal.PrintLn( GenerateNoteBox( note , ++index, 1 ) );

                    Terminal.NewLine( );
                }

			//
			// ─── EDIT NOTE AS INDEX ──────────────────────────────────────────
			//

				public static void EditNoteAtIndex ( string indexString ) {
					try {
						int index =
                            int.Parse( indexString ) - 1;
						string[ ] notes =
                            LoadNotes( );

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
			// ─── STORE NOTE LIST ─────────────────────────────────────────────
			//

				public static void StoreNoteList ( string[ ] noteList ) {
					try {
						using( StreamWriter writer = new StreamWriter ( note_file_address ) ) {
							foreach ( string note in noteList )
								writer.WriteLine( note + "\n\n" );
						}
					} catch {
						Report( "Could not store the notes..." );
					}
				}

			//
			// ─── PRINT THE HELP PAGE ─────────────────────────────────────────
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

                    string[ ] string_parts = LoadNotes( );

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
                                foreach ( string arg in args )
                                    if ( index.ToString( ) == arg )
                                        is_not_to_be_removed = false;


                                if ( is_not_to_be_removed )
                                    writer.WriteLine( element + "\n" );
                            }
                        }
                    }
				}

			//
			// ─── NEW NOTE INTERFACE ─────────────────────────────────────────────────────────────────────────────
			//

				public static string GetNoteFromInterface ( string load_note_with_text = "" ) {

                    Terminal.PrintLn(
                        Kary.Text.TextShapes.Box( "NOTE" , Terminal.Width - 5 , 1 , 0 , TextJustification.Left )
                    );


                    Terminal.X = 7; Terminal.Y -= 3;
                    Terminal.Print( '┬' );

                    Terminal.X--; Terminal.Y++;
                    Terminal.Print( '│' );

                    Terminal.X--; Terminal.Y++;
                    Terminal.Print( '┴' );


                    Terminal.Y --;
                    Terminal.X = 9;
                    string new_note = Terminal.TextBox( Terminal.Width - 12 , load_note_with_text );
                    Terminal.Y++;

                    return new_note;
				}

			//
			// ─── NEW NOTE SAVER ─────────────────────────────────────────────────────────────────────────────────
			//

				public static void StoreNewNote ( string new_note_text ) {
					try {
						using ( var reader = new StreamReader ( note_file_address ) ) {
							string note_file_string =
                                reader.ReadToEnd( );
							reader.Close( );

							try {
								using ( var writer = new StreamWriter ( note_file_address ) ) {
									writer.Write ( note_file_string + "\n\n" + new_note_text );
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
					foreach ( var arg in args )
						note_text += arg + " ";

					return note_text;
				}

			//
			// ─── EDIT COMMAND NO ARG ERROR ──────────────────────────────────────────────────
			//

				public static void PrintEditNoArgError ( ) {
					int countOfNotes =
                        LoadNotes( ).Length;
					Random rnd =
                        new Random( );
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
					int countOfNotes =
                        LoadNotes( ).Length;
					Random rnd =
                        new Random( );

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
					Terminal.NewLine( );
					Terminal.Red( );
					Terminal.PrintLn( "  O P E R A T I O N   F A I L I U R E\n" );
					Terminal.Reset( );
					string msg =
                        Utilities.Perpend( Justify.Left( message , 50 ) , "  │ " );
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

					if ( !LoadPathAddress( ) )
						return;


					//
					// PARSING THE ARGUMENTS
					//

					if ( args.Length == 0 )
						PrintNote( );
					else if ( args.Length >= 1 )
						if ( args[ 0 ] == "help" )
							PrintHelpPage( );
						else if ( args[ 0 ] == "edit" && args.Length == 1 )
							PrintEditNoArgError( );
						else if ( args[ 0 ] == "rm" || args[ 0 ] == "remove" )
							if ( args.Length == 1 )
								PrintRemoveNoArgError( );
							else
								RemoveNotes( args );

						else if ( args[ 0 ] == "add" || args[ 0 ] == "new" )
					        StoreNewNote( GetNoteFromInterface( ) );
						else if ( args.Length == 2 && args[ 0 ] == "edit" )
					        EditNoteAtIndex( args[ 1 ] );
						else
							StoreNewNote( GetNewNoteFromArgs( args ) );
				}

			// ────────────────────────────────────────────────────────────────────────────────────────────────────

		}
	}

// ────────────────────────────────────────────────────────────────────────────────
