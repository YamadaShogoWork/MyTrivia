# ファイル配置指示書

作成日: 2026-05-16

---

## 設計書ファイル

Claudeが出力した `.md` ファイルは `docs/` フォルダに置いてください。

| ファイル名 | 置き場所 |
|---|---|
| db_design.md | `MyTrivia/docs/db_design.md` |
| basic_design.md | `MyTrivia/docs/basic_design.md` |
| mockup_procedure.md | `MyTrivia/docs/mockup_procedure.md` |
| fileplace_guide.md（本ファイル） | `MyTrivia/docs/fileplace_guide.md` |

---

## モックアップファイル

Claudeが出力したモックアップの `.html` ファイルは `docs/mockups/` フォルダに置いてください。

| ファイル名 | 置き場所 | 対応画面 |
|---|---|---|
| SC-01_home.html | `MyTrivia/docs/mockups/SC-01_home.html` | ホーム |
| SC-shared_nav.html | `MyTrivia/docs/mockups/SC-shared_nav.html` | 共通ナビゲーション |
| SC-02_trivia_list.html | `MyTrivia/docs/mockups/SC-02_trivia_list.html` | 雑学一覧 |
| SC-03_trivia_detail.html | `MyTrivia/docs/mockups/SC-03_trivia_detail.html` | 雑学詳細 |
| SC-04_trivia_create.html | `MyTrivia/docs/mockups/SC-04_trivia_create.html` | 雑学登録 |
| SC-05_trivia_edit.html | `MyTrivia/docs/mockups/SC-05_trivia_edit.html` | 雑学編集 |
| SC-06_quiz_list.html | `MyTrivia/docs/mockups/SC-06_quiz_list.html` | クイズ一覧 |
| SC-07_quiz_play.html | `MyTrivia/docs/mockups/SC-07_quiz_play.html` | クイズ出題 |
| SC-08_quiz_create.html | `MyTrivia/docs/mockups/SC-08_quiz_create.html` | クイズ登録 |
| SC-09_quiz_edit.html | `MyTrivia/docs/mockups/SC-09_quiz_edit.html` | クイズ編集 |
| SC-10_video_list.html | `MyTrivia/docs/mockups/SC-10_video_list.html` | 動画一覧 |
| SC-11_video_create.html | `MyTrivia/docs/mockups/SC-11_video_create.html` | 動画登録 |
| SC-12_video_edit.html | `MyTrivia/docs/mockups/SC-12_video_edit.html` | 動画編集 |
| SC-13_tag_list.html | `MyTrivia/docs/mockups/SC-13_tag_list.html` | タグ一覧 |
| SC-14_tag_create.html | `MyTrivia/docs/mockups/SC-14_tag_create.html` | タグ登録 |
| SC-15_tag_edit.html | `MyTrivia/docs/mockups/SC-15_tag_edit.html` | タグ編集 |

---

## 完成後のフォルダ全体像

```
MyTrivia/
├── docs/
│   ├── db_design.md
│   ├── basic_design.md
│   ├── mockup_procedure.md
│   ├── fileplace_guide.md
│   └── mockups/
│       ├── SC-01_home.html
│       ├── SC-shared_nav.html
│       ├── SC-02_trivia_list.html
│       ├── SC-03_trivia_detail.html
│       ├── SC-04_trivia_create.html
│       ├── SC-05_trivia_edit.html
│       ├── SC-06_quiz_list.html
│       ├── SC-07_quiz_play.html
│       ├── SC-08_quiz_create.html
│       ├── SC-09_quiz_edit.html
│       ├── SC-10_video_list.html
│       ├── SC-11_video_create.html
│       ├── SC-12_video_edit.html
│       ├── SC-13_tag_list.html
│       ├── SC-14_tag_create.html
│       └── SC-15_tag_edit.html
│
├── src/
│   └── MyTrivia/             ← dotnet new webapp で生成されるプロジェクト
│       ├── Controllers/
│       ├── Models/
│       ├── Views/
│       ├── Data/
│       ├── Migrations/
│       ├── wwwroot/
│       ├── appsettings.json
│       └── Program.cs
│
├── .gitignore
└── README.md
```

---

## フォルダの作り方

Windowsのエクスプローラーで手動で作ってもOKですが、VS Codeのターミナルで以下を実行すると一気に作れます。

```bash
mkdir -p MyTrivia/docs/mockups
mkdir -p MyTrivia/src
cd MyTrivia/src
dotnet new webapp -n MyTrivia
cd ../..
dotnet new gitignore -o MyTrivia
```

---

## GitHubにあげるときの注意

以下のファイルは `.gitignore` に含まれるため自動的に除外されます。

| 除外されるもの | 理由 |
|---|---|
| `src/MyTrivia/bin/` | ビルド成果物 |
| `src/MyTrivia/obj/` | ビルド成果物 |
| `appsettings.Development.json` | DB接続パスなどの秘匿情報 |

`docs/` フォルダの設計書・モックアップはすべてGitHubにあげてOKです。
