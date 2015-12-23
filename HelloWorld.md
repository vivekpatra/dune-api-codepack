# Introduction #

I'll quickly show you a couple of tricks that should get you started. All methods and properties follow the same patterns as described here, so it should be fairly easy to figure out the rest.

# Getting started (code examples) #
The codepack has one important type, which is the [Dune](https://dl.dropbox.com/u/14454764/DuneAPICodePack/Help/html/6f88139e-7101-813f-0494-03be50184e0d.htm) class. This class has all the methods and properties that do the dirty work. Once you create an instance, you can get/set various properties (such as playback volume), or use one of the more advanced methods.

Assuming that you have downloaded the project and referenced it from your startup project, it's easy to get started:

1. place this at the top of your code
```
Imports SL.DuneAPICodePack
Imports SL.DuneAPICodePack.DuneUtilities
Imports SL.DuneAPICodePack.DuneUtilities.ApiWrappers
```


2. In your method of choice, declare a Dune object and instantiate it. This can be done by specifying the connection details in one of the constructor overloads, or by calling the connect method later. I do advise you to connect the instance before doing anything else.
Be warned that you may get a socket exception if the host lookup fails, usually when you specify a wrong address.

```
Dim dune As New Dune

Try
    dune.Connect("192.168.1.139")
Catch ex As SocketException
    Console.WriteLine(ex.Message)
End Try
```

3. If the instance connected without blowing up your machine, you are now able to start sending commands.

Some methods will take raw integer or string parameters, and that might be somewhat confusing at first. What you need to do is use one of the constants that are enumerated in [SL.DuneApiCodePack.Constants](https://dl.dropbox.com/u/14454764/DuneAPICodePack/Help/html/f6904b1f-181a-136c-923c-001e9d51be0c.htm), depending on which method you are calling.

Example: Use [dune.RemoteControl](https://dl.dropbox.com/u/14454764/DuneAPICodePack/Help/html/c7832a50-62c5-73e9-ef1b-c7f31b98290d.htm) to emulate button presses. Use one of the constants enumerated in `Constants.RemoteControls` as the parameter.


```
dune.RemoteControl.Push(Constants.RemoteControls.BigRemoteButtonValues.Left)
dune.RemoteControl.Push(Constants.RemoteControls.BigRemoteButtonValues.Down)
dune.RemoteControl.Push(Constants.RemoteControls.BigRemoteButtonValues.Enter)
```


Use other properties and methods to set various other options:
```
dune.PlaybackVolume = 100 ' sets the playback volume to 100%'
dune.PlaybackPosition = New TimeSpan(0, 0, 30) ' sets the playback position to 00:00:30
```

To do something more advanced, you can build your own commands:

```
Dim command As New StartPlaybackCommand(dune, "storage_label://DuneHDD/Example.iso")
Dim result As CommandResult = command.GetResult

If result.CommandStatus = Constants.CommandStatusValues.Ok Then
    Console.WriteLine("Doing it right!")
ElseIf result.CommandStatus = Constants.CommandStatusValues.Failed Then
    Console.WriteLine("Doing it wrong!")
ElseIf result.CommandStatus = Constants.CommandStatusValues.Timeout Then
    Console.WriteLine("All this waiting for nothing!")
End If
```


There are far more possibilities than what is described here, but you're really wasting time by reading this rather than just trying the code. To get a better overview, use the Object Browser in visual studio (CTRL+Alt+J). Or check out the full help file [here](https://dl.dropbox.com/u/14454764/DuneAPICodePack/Help/Index.html).