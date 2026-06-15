# Claude Code 指示書 - MyTrivia 残作業

## プロジェクト概要

ASP.NET Core MVC + SQLite + Entity Framework Core で作った個人用Webアプリ。
YouTubeチャンネル「ゆる言語学ラジオ」の言語学雑学を管理するシステム。

---

## プロジェクトの場所

```
C:\Work\MyTrivia\src\MyTrivia
```

---

## やること

以下の4ファイルを作成してください。既存のViewファイルのデザインに合わせて実装してください。

### 1. Views/Tag/Edit.cshtml

タグ編集画面。以下の仕様で作成してください。

- `@model Tag`
- `ViewData["ActiveNav"] = "Tag"`
- Toolbarに「← 一覧に戻る」ボタン（`asp-action="Index"`）
- フォーム（`asp-action="Edit"`, `asp-route-id="@Model.Id"`, method="post"）
  - hidden: `Id`
  - フィールド：タグ名（`TagName`、必須）
  - ヒント：`このタグは現在 @ViewBag.TriviaCount 件の雑学に使われています。`
  - 警告メッセージ：`タグ名を変更すると、このタグが付いたすべての雑学に反映されます。`（赤文字）
- ボタン：「更新する」（primary）、「キャンセル」（`asp-action="Index"`）

### 2. Views/Video/Index.cshtml

動画一覧画面。以下の仕様で作成してください。

- `@model IEnumerable<Video>`
- `ViewData["ActiveNav"] = "Video"`
- `ViewBag.TriviaCounts` は `Dictionary<int, int>`（動画IDをキーに雑学数が入る）
- Toolbarに「＋ 新規登録」ボタン（`asp-action="Create"`）
- テーブルのカラム：動画タイトル / URL / 公開日 / 雑学数 / 操作
  - URLは `<a href="@video.VideoUrl" target="_blank">` でリンク表示（長い場合は35文字で切る）
  - 公開日は `PublishedAt.HasValue` で確認してNULLなら「—」
  - 雑学数はアンバーのバッジで表示
  - 操作：編集ボタン（`asp-action="Edit"`）、削除ボタン（confirm付きPOSTフォーム）
    - 削除confirmメッセージ：`削除しますか？紐づく雑学の参照動画はNULLになります。`
- データなしの場合：`動画が登録されていません。`

### 3. Views/Video/Create.cshtml

動画登録画面。以下の仕様で作成してください。

- `@model Video`
- `ViewData["ActiveNav"] = "Video"`
- Toolbarに「← 一覧に戻る」ボタン（`asp-action="Index"`）
- フォーム（`asp-action="Create"`, method="post"）
  - フィールド：動画タイトル（`VideoTitle`、必須）
  - フィールド：YouTube URL（`VideoUrl`、必須）、ヒント：`動画のURLをそのまま貼り付けてください。`
  - フィールド：公開日（`PublishedAt`、type="date"、任意）
- ボタン：「登録する」（primary）、「キャンセル」（`asp-action="Index"`）

### 4. Views/Video/Edit.cshtml

動画編集画面。以下の仕様で作成してください。

- `@model Video`
- `ViewData["ActiveNav"] = "Video"`
- Toolbarに「← 一覧に戻る」ボタン（`asp-action="Index"`）
- フォーム（`asp-action="Edit"`, `asp-route-id="@Model.Id"`, method="post"）
  - hidden: `Id`
  - フィールド：動画タイトル（`VideoTitle`、必須）
  - フィールド：YouTube URL（`VideoUrl`、必須）
  - フィールド：公開日（`PublishedAt`、type="date"、任意）
- ボタン：「更新する」（primary）、「キャンセル」（`asp-action="Index"`）

---

## デザインの共通ルール

既存の `_Layout.cshtml` にCSSが定義されています。以下のクラスを使ってください。

| クラス名 | 用途 |
|---|---|
| `.toolbar` | ページ上部のツールバー |
| `.btn` | 通常ボタン |
| `.btn.primary` | アンバー色のメインボタン |
| `.btn.danger` | 赤色の削除ボタン |
| `.act` | テーブル内の小さいボタン |
| `.form` | フォームのラッパー |
| `.field` | 各入力フィールドのラッパー |
| `.field-label` | フィールドのラベル |
| `.req` | 必須マーク（赤い*） |
| `.hint` | ヒントテキスト（グレー小文字） |
| `.form-actions` | 登録・キャンセルボタンのエリア |
| `.validation-error` | バリデーションエラーメッセージ |

アクセントカラー：`#BA7517`（アンバー）
タグのスタイル：`background:#FAEEDA; color:#633806`

---

## 既存ファイルの参考

`Views/Tag/Create.cshtml` や `Views/Trivia/Edit.cshtml` を参考にしてください。

---

## 完成後にやること

```bash
cd C:\Work\MyTrivia\src\MyTrivia
dotnet run
```

ブラウザで動作確認後、以下でGitHubにpushする。

```bash
cd C:\Work\MyTrivia
git add .
git commit -m "Video・TagのView追加、全機能完成"
git push
```
