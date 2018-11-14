# Roland INTEGRA-7 Librarian and Editor

Roland INTEGRA-7 is a synthesizer module, read about it here:
https://www.roland.com/global/products/integra-7/

I have written this program for Windows 10 as a UWP solution.
It is very useful, and I think many users of INTEGRA-7 would like it.
However, most users of the INTEGRA-7 uses a MacBook, an iPad, or even an Android tablet, so
now my plan is to re-write it using the Xamarin platform.

All that possibly can be implemented in common code goes into the
portable project.

UI is handled in the file UIHandler.cs in the portable project, where
all controls are dynamically created. The original program was split
up in a few pages, and xaml code was used in some pages, but it has
an engine for creating UI controls, so that one is beeing adapted to
Xamarin and will be used for all user controls.

Also, there will not be separate pages. The UI handler will simple
show the content of the main page with different controls, and create a page only at first usage. Each page is a StackLayout and the property IsVisible is used to show a page while hiding all the other pages.

The only parts that needs to be in the native projects are, as far
as I have realized for now, Images, file handling and the MIDI handler.

Most images will be the same and can be copied or maybe referred to
from all native projects, if possible.

The MIDI code is very different. I can use the UWP MIDI code as is, but
had to write a new code for the MacBook. It seems to work, but I suspect
I will find bugs in the future. Hopefully the MacBook MIDI implementation
will also work for the iPad. I do not have an iPad and have thus not yet
tested that, nor implemented it yet.

I usually enter comments in code to explain what it does and how it works.
