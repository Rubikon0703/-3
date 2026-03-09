open System

// Безопасное преобразование строки в int 
let tryParseInt (s: string) =
    match Int32.TryParse(s) with
    | (true, value) -> Some value
    | _ -> None

// Для корректной проверки ввода
let rec readIntList () =
    printf "Введите список целых чисел через пробел: "
    let input = Console.ReadLine()
    let parts = input.Split([| ' ' |], 
        StringSplitOptions.RemoveEmptyEntries)
    
    // Для преобразования в Int
    let rec parseList acc remaining =
        match remaining with
        | [] -> Some (List.rev acc)  
        | x::xs ->
            match tryParseInt x with
            | Some num -> parseList (num::acc) xs
            | None -> None  
    
    match parseList [] (List.ofArray parts) with
    | Some nums -> nums
    | None ->
        printfn "Ошибка: введите только целые числа, разделенные пробелами."
        readIntList ()  


let rec readInt prompt =
    printf "%s" prompt
    let input = Console.ReadLine()
    match tryParseInt input with
    | Some value -> value
    | None ->
        printfn "Ошибка: введите целое число."
        readInt prompt

// Поиск первой цифры числа 
let firstDigit (n: int) =
    let absN = abs n
    let rec loop x =
        if x < 10 then x
        else loop (x / 10)
    loop absN

// Задание 1 
let searchFirst (numbers: seq<int>) =
    let firstDigits = numbers |> Seq.map firstDigit
    printfn "Исходная последовательность: %A" numbers
    printfn "Последовательность первых цифр: %A" firstDigits

// Задание 2 
let counting (numbers: seq<int>) =
    let target = readInt "Введите число для подсчёта: "
    let count =
        numbers
        |> Seq.fold (fun acc x -> if x = target then acc + 1 else acc) 0
    printfn "%d встречается %d раз(а)" target count

// Точка входа
[<EntryPoint>]
let main argv =
    let numbers = readIntList ()      
    printfn ""
    searchFirst (Seq.ofList numbers)
    printfn ""
    counting (Seq.ofList numbers)
    0