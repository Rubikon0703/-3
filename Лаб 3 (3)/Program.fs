open System
open System.IO
let rec promptForPath() =
    printf "Введите путь к директории: "
    let path = Console.ReadLine()
    if String.IsNullOrWhiteSpace path then
        eprintfn "Ошибка: путь не указан!"
        promptForPath()
    elif not (Directory.Exists path) then
        eprintfn "Ошибка: Директория '%s' не существует!" path
        printfn "Попробуйте снова."
        promptForPath()
    else
        path


let getDirectory (argv: seq<string>) =
    match Seq.tryHead argv with
    | Some arg when Directory.Exists arg -> arg
    | Some arg ->
        eprintfn "Ошибка: Директория '%s' не существует!" arg
        promptForPath()
    | None -> promptForPath()


let rec findTxtFiles dir =
    seq {
      
        let files = Directory.EnumerateFiles(dir, "*.txt")
        for file in files do
            yield file

        let subDirs = Directory.EnumerateDirectories(dir)
        for subDir in subDirs do
            yield! findTxtFiles subDir
    }


let printResults results =
    if Seq.isEmpty results then
        printfn "Файлы .txt не найдены."
    else
        results |> Seq.iter (printfn "%s")

[<EntryPoint>]
let main argv =
    let directory = getDirectory argv
    let fullPath = Path.GetFullPath directory
    printfn "Поиск файлов в '%s' и его подкаталогах:" fullPath
    findTxtFiles fullPath
    |> printResults
    0