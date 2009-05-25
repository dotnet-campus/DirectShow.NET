As with any code that is labeled "sample," you should be clear about what level of quality you are expecting.
While there are no bugs or gotchas in this code TO MY KNOWLEDGE, that doesn't mean there aren't any.  You should
review the code yourself, and test it for your specific purposes.

This is an improved version of the GSSF filter.  In addition to the capabilities of that filter,
it also allows the c# code to support IMediaSeeking.  It uses a different CLSID and IID than the
old GSSF.

In order to rebuild this filter, you will need to first build the c++ baseclasses.  Search
google if you need directions on how to do that.  Or you can just use the pre-built version included
with the samples.

As with all COM objects, the filter must be registered with COM before it will work.  If you
build the project with Visual Studio, it will register it for you.  Otherwise, you can run

    regsvr32 gssf2.dll
    
or

    regsvr32 gssf2-64.dll

