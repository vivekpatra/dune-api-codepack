# bluray\_navigation #
## Summary ##

`bluray_navigation` is used to navigate Blu-ray menus.

## Syntax ##

| **Parameter**         | **Value(s)**        | **Example**             | **API Version(s)** |
|:----------------------|:--------------------|:------------------------|:-------------------|
| [cmd](Cmd.md)           | bluray\_navigation  | cmd=bluray\_navigation  | ≥ 1                |
| [action](Action.md)     | {action}            | action=enter            | ≥ 1                |
| [[timeout](Timeout.md)] | {n}                 | timeout=20              | ≥ 1                |

## Examples ##

`http://dune/cgi-bin/do?cmd=bluray_navigation&action=left`

`http://dune/cgi-bin/do?cmd=bluray_navigation&action=right`

`http://dune/cgi-bin/do?cmd=bluray_navigation&action=up`

`http://dune/cgi-bin/do?cmd=bluray_navigation&action=down`

`http://dune/cgi-bin/do?cmd=bluray_navigation&action=enter`

## Notes ##

I see no reason why you shouldn't just use [ir\_code](IrCodeCommand.md) commands instead.