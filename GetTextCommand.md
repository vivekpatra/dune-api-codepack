# get\_text #
## Summary ##

`get_text` gets the text from the active text field.

## Syntax ##

| **Parameter**         | **Value(s)** | **Example**    | **API Version(s)** |
|:----------------------|:-------------|:---------------|:-------------------|
| [cmd](Cmd.md)           | get\_text    | cmd=get\_text  | ≥ 3                |
| [[timeout](Timeout.md)] | {n}          | timeout=20     | ≥ 1                |

## Examples ##

`http://dune/cgi-bin/do?cmd=get_text`

## Notes ##

If there is no active text field, the command fails.