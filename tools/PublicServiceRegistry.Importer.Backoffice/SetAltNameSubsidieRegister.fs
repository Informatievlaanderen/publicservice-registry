module SetAltNameSubsidieRegister

  open FSharp.Data

  open CommonLibrary
  open PublicService.Backoffice

  [<Literal>]
  let sampleFile = "Import/SetAltLabelSubsidieRegister/batch_20190220_sarah_altname_subsidieregister.txt"

  type AltNamesForSubsidieRegister =
    CsvProvider<Sample      = sampleFile,
                Separators  = "|",
                HasHeaders  = true,
                Encoding    = "utf-8"
                >

  type Counter = { Total: int; Successes: int }

  let getRows (batchFile:string) =
    let alternativeNames = AltNamesForSubsidieRegister.Load(batchFile)
    alternativeNames.Rows

  let importRow counter apiBaseUrl token rowCount cursorTop (row:AltNamesForSubsidieRegister.Row) =

    let result =
      putAltNameSubsidieRegister apiBaseUrl token row.``Alternatieve naam subsidieregister`` row.Id

    let counter =
      match result with
      | Success _ -> { Total = counter.Total + 1; Successes = counter.Successes + 1 }
      | Failure _ -> { Total = counter.Total + 1; Successes = counter.Successes }

    System.Console.CursorTop <- cursorTop
    printfn ""
    printfn "\rProcessing %i/%i ..." counter.Total rowCount
    printfn "\rSuccesses: %i/%i ..." counter.Successes rowCount
    printfn "\rErrors: %i/%i ..." (counter.Total - counter.Successes) rowCount

    (row.Id, result), counter

  let importRows apiBaseUrl token rows =
    let rowCount = Seq.length rows
    let cursorTop = System.Console.CursorTop
    Seq.mapFold (fun i row -> importRow i apiBaseUrl token rowCount cursorTop row) { Total = 0; Successes = 0 } rows
    |> fun (x, _) -> x
    |> Seq.toList
