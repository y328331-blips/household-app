# household-app

自分用の家計簿アプリ。日付・項目・金額を記録して、収支の一覧とグラフを見られます。

## 技術構成

- .NET 8 / Blazor WebAssembly + ASP.NET Core
- SQLite (Entity Framework Core)

## フォルダ構成

```
household-app/
├── README.md                 このファイル
├── CLAUDE.md                 リポジトリ情報・作業ルール
├── HouseholdApp.sln          ソリューションファイル
│
├── HouseholdApp/              サーバー側(API・DB・画面のホスト)
│   ├── Program.cs               起動処理、/api/transactions のAPI定義
│   ├── Data/
│   │   └── AppDbContext.cs      EF CoreのDB定義(Transactionsテーブル)
│   ├── Components/              画面の外枠(レイアウト・ルーティング)
│   ├── wwwroot/app.css          全体のスタイル(色・レイアウト)
│   └── appsettings.json         DB接続文字列などの設定
│
└── HouseholdApp.Client/       ブラウザ側(Blazor WebAssembly、実際の画面)
    ├── Pages/
    │   ├── List.razor           一覧画面(収支サマリー・グラフ・記録一覧)
    │   └── Input.razor          入力画面(日付・項目・金額の登録)
    ├── Models/
    │   ├── TransactionItem.cs   1件の記録の型(日付・項目・金額・種別)
    │   └── TransactionType.cs   種別(支出/収入)
    └── Services/
        └── TransactionApiClient.cs  サーバーAPIへの読み書き
```

**迷ったらここを見る:**
- 画面の見た目・動きを変えたい → `HouseholdApp.Client/Pages/`
- 色・フォントサイズなど全体のデザインを変えたい → `HouseholdApp/wwwroot/app.css`
- 記録項目(データの持ち方)を変えたい → `HouseholdApp.Client/Models/TransactionItem.cs`
- DBの構造を変えたい → `HouseholdApp/Data/AppDbContext.cs`

## ローカルでの起動方法

```bash
cd HouseholdApp
dotnet run --urls http://localhost:5239
```

ブラウザで `http://localhost:5239` を開く。
