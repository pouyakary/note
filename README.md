
# Note

A very simple terminal note tool, I developed for myself but you can benefit from it too. It's designed to work in my computer as the path of the note file is hard-coded to the base of the software. Also you may need to compile a [run.vala](https://github.com/pmkary/run) 

## How to use it

After you installed it you can read your notes using `note` command in terminal:

```
% note
```

That will result:


```
Pouyas-MacBook-Pro:~ pmk$ note

 I → AIS 2XII for English class is needed 

 II → kary systems website needs work to do 

 III → negative index for arrays 

 IV → There is a need for a JavaScript
      infrastructure in Arendelle Adgar
      engine. Also basic html tools for
      creating the grid screen 

```

To add a new note you can simply write your note after the `note command` for example adding `hello world` will be:

```
% note hello world
```

For the notes that inculde special characters bash may have problem with like `'` you can use the command:

```
% note add
```

And then a prompt like this will open:

```
┌──────────┬─────────────────────────────────────────────────────┐
│ NEW NOTE │                                                     │
└──────────┴─────────────────────────────────────────────────────┘
```

There you can write a note like whatever that is in your mind!


Now if we check the notes again we there hello world is here:

```
Pouyas-MacBook-Pro:~ pmk$ note

 I → AIS 2XII for English class is needed 

 II → kary systems website needs work to do 

 III → negative index for arrays 

 IV → There is a need for a JavaScript
      infrastructure in Arendelle Adgar
      engine. Also basic html tools for
      creating the grid screen 

 V → hello world 
```

To remove the note you can use `note rm` command, For example you want to remove note 2 and 5, You should use the `note rm` this way:

```
% note rm 2 5
```

And if we run the note again:

```
Pouyas-MacBook-Pro:~ pmk$ note

 I → AIS 2XII for English class is needed 

 II → negative index for arrays 

 III → There is a need for a JavaScript
       infrastructure in Arendelle Adgar
       engine. Also basic html tools for
       creating the grid screen 
```

<br><br>

## Setting it up
#### Step 1
First of all you'll need to make an empty text file and name it whatever you want and save it in whatever form you want. In a UNIX system I would recommend perpending the name with a dot so the file goes hidden. Now for example the path to our file is 

```
/Users/pmk/something/.notes
```

#### Step 2
Now open up the solution file of the project and in the `program.cs` file find this line:

```C#
static string note_file_address = "/Users/pmk/Dropbox/.notes";
```

As you see placing the file in a folder that is being synced with Internet makes you able to have something like Evernote!

The line must be `24` or somewhere near that. Change it to the path of your file:

```
static string note_file_address = "/Users/pmk/something/.notes";
```

**NOTE: ** Remember that you'll need [Panther Framework](https://github.com/karysystems/panther).

#### Step 3

Now build the note and copy the path of your binary like this:

```
/Users/pmk/somewhere/note/note/bin/release/Note.exe
```

#### Step 4
Most of our job is done, We only need to make a shortcut for it. If you're using a good base like Linux you can simply edit the `.bashrc` file and add and alias to the binary but using this way you can do it anywhere including OS X and all UNIX based systems supporting 

Download a copy of [run.vala](https://github.com/pmkary/run). Now replace this line:

```C#
string ArcadeCommand = "<ReplaceWithThePathOfYourArcadeDirectoryAndAddress>";
```

With this:

```C#
string ArcadeCommand = "mono '/Users/pmk/somewhere/note/note/bin/release/Note.exe'";
```

**NOTE : ** Remember that the string must be `mono ' + the path of your file + '` like the example in the top

Now compile the file using:

```
valac -o name --pkg posix ./run.vala
```

So the shortcut is ready now. You can install it in `/bin` or `/usr/local/bin` in a unix system (and there is no point to create a shortcut for Windows systems anyway...) like this:

```
sudo mv ./note /bin
```

Or the local bin (whatever...)

#### 5
You're all done!

<br><br>

## License

```
Note - Super minimal terminal note app
Copyright (c) 2015 Pouya Kary <k@arendelle.org>


This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
```

