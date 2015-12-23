# set\_text #
## Summary ##

`set_text` replaces the text in the active text field with the specified value.

## Syntax ##

| **Parameter**         | **Value(s)** | **Example**          | **API Version(s)** |
|:----------------------|:-------------|:---------------------|:-------------------|
| [cmd](Cmd.md)           | set\_text    | cmd=set\_text        | ≥ 3                |
| [text](Text.md)         | {text}       | text=Lorem%20Ipsum   | ≥ 3                |
| [[timeout](Timeout.md)] | {n}          | timeout=20           | ≥ 1                |

## Examples ##

`http://dune/cgi-bin/do?cmd=set_text&text=Hello%20World!`

## Notes ##

If there is no active text field, the command fails.