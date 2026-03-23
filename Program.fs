open System

let tryParseInt (s: string) =
    match Int32.TryParse(s) with
    | (true, value) -> Some value
    | _ -> None

let rec readInt prompt =
    printf "%s" prompt
    let input = Console.ReadLine()
    match tryParseInt input with
    | Some value -> value
    | None ->
        printfn "Ошибка: введите целое число."
        readInt prompt

let rec readIntSequence () =
    seq {
        let input = Console.ReadLine()
        
        if input <> "" then
            match tryParseInt input with
            | Some value ->
                yield value
                yield! readIntSequence ()
            | None ->
                printfn "Ошибка: введите целое число."
                yield! readIntSequence ()
    }

let firstDigit (n: int) =
    let absN = abs n
    let rec loop x =
        if x < 10 then x
        else loop (x / 10)
    loop absN

let searchFirst (numbers: seq<int>) =
    numbers |> Seq.map firstDigit


let counting (numbers: seq<int>) (target: int) =
    numbers |> Seq.fold (fun acc x -> if x = target then acc + 1 else acc) 0

[<EntryPoint>]
let main argv =
    printfn "Введите числа (пустая строчка для завершения):"
    
    let numbers = readIntSequence ()
    let numbersCache = numbers |> Seq.cache
    
    numbersCache |> Seq.iter (fun x -> printfn "%d -> %d" x (firstDigit x))
  
    printf "Последовательность первых цифр: "
    let firstDigits = searchFirst numbersCache
    firstDigits |> Seq.iter (printf "%d ")

    printfn "\n"
    let target = readInt "Введите число для подсчёта: "
    let count = counting numbersCache target
    printfn "число %d встречается %d раз(а)" target count
    
    0