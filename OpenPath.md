# open\_path #
## Summary ##

`open_path` sets the navigator to the specified view.

## Syntax ##

| **Parameter**         | **Value(s)**  | **Example**             | **API Version(s)** |
|:----------------------|:--------------|:------------------------|:-------------------|
| [cmd](Cmd.md)           | open\_path    | cmd=open\_path          | ?                  |
| [url](Url.md)           | {path}        | url=root://favorites    | ?                  |
| [[timeout](Timeout.md)] | {n}           | timeout=20              | ≥ 1                |

## Examples ##

Set the navigator to the favorites tab:

`http://dune/cgi-bin/do?cmd=open_path&url=root://favorites`