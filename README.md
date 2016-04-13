
![](http://kary.us/GitHubWideImages/note/screen.png) 

# Note

I have always hated every single note / reminder / whatever application that was around. After writing the [Kary Framework](https://github.com/karyfoundation/framework-base), I started creating a very simple and pretty note software so that I could use for real. This is the result. 

## What is special about note?

- Super easy to work with
- Very minimal
- One of the most complicated Kary Framework UI codes in history and really the most complicated Terminal UI layout of all time! (have you noticed the float divs inside it?)
- Fully cross platform
- Note uses plain text to store it's notes hence the end result is fully printable and rebate
- Plain text storage &rightarrow; so that you can put your storage in your Dropbox, iCloud, Google Drive or whatever you have!
- Fully cross platform! with a cloud synced system you can even have your notes synced all over your devices!

## How to use it

#### Seeing your notes

After you installed it you can read your notes using `note` command in terminal:

```
% note
```

That will result:

![](http://kary.us/GitHubWideImages/note/screen.png) 

<br/>

#### Adding notes

To add a new note you can simply write your note after the `note command` for example adding `hello world` will be:

```
% note hello world
```

For the notes that inculde special characters bash may have problem with like `'` you can use the command:

```
% note add
```

And then a prompt like this will open:

![](http://kary.us/GitHubWideImages/note/add.png) 

There you can write a note like whatever that is in your mind!

<br/>

#### Removing notes
To remove the note you can use `note rm` command, For example you want to remove note 2 and 5, You should use the `note rm` this way:

```
% note rm 2 5
```

<br/>

#### Editing notes
Editing notes is just as easy. All you have to do to edit a note is to specify the note index after the `edit` command:

```
% note edit 12
```

And then thanks to the Kary Frameworks's text box implementation, the pervious note can be loaded into the input box as:

![](http://kary.us/GitHubWideImages/note/edit.png) 



<br><br>

## Setting it up
#### Step 1
First of all you'll need to make an empty text file and name it whatever you want and save it in whatever form you want. In a UNIX system I would recommend perpending the name with a dot so the file goes hidden. Now for example the path to our file is 

```
~/.notes
```

This file is going to contain your notes so if you place it under your dropbox folder for example you'll get dropbox sync as well!

<br/>
#### Step 2
Create a text file with this address:

```
~/.notepath
```
And write the address of your storage file in it. In our previous file we used `~/.notes` so let's use that (remember just the address no spacing of any kind - `tab`, `space`, `new line` - after it and before it)

<br/>
#### Step 3

Download the note's release package from [Note's Release Page](https://github.com/pmkary/note/releases) (a zip file containing `Note.exe` and `Kary.Foundation.dll`), exact it and copy them together to whatever you want

<br/>
#### Step 4
Most of our job is done, We only need to make a shortcut for it. If you're using a good base like Linux you can simply edit the `.bashrc` file and add and alias to the binary but using this way you can do it anywhere including OS X and all UNIX based systems supporting 

Download a copy of [run.vala](https://github.com/pmkary/run). Now replace this line:

```C#
string ArcadeCommand = "<ReplaceWithThePathOfYourArcadeDirectoryAndAddress>";
```

With this:

```C#
string ArcadeCommand = "mono '<where-you-copied-your-files>'";
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

<br />
#### Step 5
You're all done! run `% note add` and have your first note!

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

