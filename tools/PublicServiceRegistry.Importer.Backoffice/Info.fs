module Info

  open System
  open System.IO

  open CommonLibrary

  let printTitle (title:string) =
    let numberOfChars = (System.Console.BufferWidth - title.Length) / 2
    let charsLeft = String.Concat(System.Linq.Enumerable.Repeat("=", numberOfChars))
    let charsRight = String.Concat(System.Linq.Enumerable.Repeat("=", numberOfChars - 1))
    printfn "%s" <| String.Concat(System.Linq.Enumerable.Repeat("=", (System.Console.BufferWidth - 1)))
    printfn "%s%s%s" charsLeft title charsRight
    printfn "%s" <| String.Concat(System.Linq.Enumerable.Repeat("=", (System.Console.BufferWidth - 1)))
    printfn ""

  let printWithColor format text =
      System.Console.ForegroundColor <- ConsoleColor.Cyan
      printf format text
      System.Console.ForegroundColor <- ConsoleColor.White

  let printFileInfo file =
    printf "We found the file "
    printWithColor "%s" <| FileInfo(file).Name
    printfn "."

  let printRowsInfo apiBaseUrl rows =

      printf "The file contains "
      printWithColor "%i rows" (Seq.length rows)
      printfn "."

      printf "The rows will be imported to "
      printWithColor "%s" apiBaseUrl
      printfn "."

      printf "Press the enter key to continue..."
      System.Console.ReadLine() |> ignore
      rows

  let printResults results =
      let resultsFile = sprintf "results_%s.txt" <| System.DateTime.Now.ToString("yyyyMMdd_HHmmss")

      printfn ""

      results
      |> Seq.filter (fun (_, r) -> isFailure r)
      |> Seq.length
      |> printfn "Number of errors: %i"

      printfn ""

      printf "Printing results to "
      printWithColor "%s" <| FileInfo(resultsFile).FullName
      printfn "."

      results
      |> Seq.sortBy (fun (id, _) -> id)
      |> Seq.map (fun (id, result) -> sprintf "%s: %A" id (mapResult result))
      |> fun lines -> System.IO.File.AppendAllLines(resultsFile, lines)
