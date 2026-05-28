# 基本設計書 - 言語学雑学管理システム

作成日: 2026-05-17
対象システム: ゆる言語学ラジオ 雑学管理アプリ

---

## 1. システム概要

YouTubeチャンネル「ゆる言語学ラジオ」で得た言語学の雑学を収集・管理するための個人用Webアプリケーション。

| 項目 | 内容 |
|---|---|
| システム名 | 言語学雑学管理システム |
| 利用者 | 個人（自分のみ） |
| 認証 | なし |
| 動作環境 | ローカル（localhost） |

---

## 2. 技術スタック

| 種別 | 採用技術 |
|---|---|
| フレームワーク | ASP.NET Core MVC |
| 言語 | C# |
| DB | SQLite |
| ORM | Entity Framework Core |
| エディタ | VS Code |
| バージョン管理 | Git / GitHub |

---

## 3. 機能一覧

### 3-1. 雑学（Trivia）

| 機能ID | 機能名 | 説明 |
|---|---|---|
| T-01 | 雑学一覧 | 登録済みの雑学を一覧表示する。タグで絞り込み・キーワード検索が可能。 |
| T-02 | 雑学詳細 | 雑学の本文・タグ・参照動画・問題文をまとめて表示する。 |
| T-03 | 雑学登録 | タイトル・本文・タグ・参照動画・参照箇所・問題文（任意）を登録する。 |
| T-04 | 雑学編集 | 登録済みの雑学を編集する。 |
| T-05 | 雑学削除 | 登録済みの雑学を削除する。関連するTriviaTagも同時に削除する。 |

### 3-2. 動画（Video）

| 機能ID | 機能名 | 説明 |
|---|---|---|
| V-01 | 動画一覧 | 登録済みのYouTube動画を一覧表示する。 |
| V-02 | 動画登録 | 動画タイトル・URL・公開日を登録する。 |
| V-03 | 動画編集 | 登録済みの動画情報を編集する。 |
| V-04 | 動画削除 | 動画を削除する。紐づく雑学のVideoIdはNULLにする（雑学は残す）。 |

### 3-3. タグ（Tag）

| 機能ID | 機能名 | 説明 |
|---|---|---|
| G-01 | タグ一覧 | 登録済みのタグを一覧表示する。 |
| G-02 | タグ登録 | タグ名を登録する。 |
| G-03 | タグ編集 | タグ名を編集する。 |
| G-04 | タグ削除 | タグを削除する。紐づくTriviaTagも同時に削除する（カスケード削除）。 |

---

## 4. 画面一覧

| 画面ID | 画面名 | URL | 対応機能 |
|---|---|---|---|
| SC-01 | ホーム | / | - |
| SC-02 | 雑学一覧 | /Trivia | T-01 |
| SC-03 | 雑学詳細 | /Trivia/Detail/{id} | T-02 |
| SC-04 | 雑学登録 | /Trivia/Create | T-03 |
| SC-05 | 雑学編集 | /Trivia/Edit/{id} | T-04 |
| SC-10 | 動画一覧 | /Video | V-01 |
| SC-11 | 動画登録 | /Video/Create | V-02 |
| SC-12 | 動画編集 | /Video/Edit/{id} | V-03 |
| SC-13 | タグ一覧 | /Tag | G-01 |
| SC-14 | タグ登録 | /Tag/Create | G-02 |
| SC-15 | タグ編集 | /Tag/Edit/{id} | G-03 |

---

## 5. 画面遷移

```
ホーム (SC-01)
├── 雑学一覧 (SC-02)
│   ├── 雑学詳細 (SC-03)
│   │   ├── 雑学編集 (SC-05)
│   │   └── [削除] → 雑学一覧へ
│   └── 雑学登録 (SC-04) → 雑学一覧へ
│
├── 動画一覧 (SC-10)
│   ├── 動画登録 (SC-11) → 動画一覧へ
│   └── 動画編集 (SC-12) → 動画一覧へ
│
└── タグ一覧 (SC-13)
    ├── タグ登録 (SC-14) → タグ一覧へ
    └── タグ編集 (SC-15) → タグ一覧へ
```

---

## 6. フォルダ構成

```
MyTrivia/
├── docs/
│   ├── db_design.md
│   ├── basic_design.md
│   ├── mockup_procedure.md
│   ├── fileplace_guide.md
│   ├── handover_memo.md
│   └── mockups/
│       ├── SC-01_home.html
│       ├── SC-02_trivia_list.html
│       ├── SC-03_trivia_detail.html
│       ├── SC-04_trivia_create.html
│       ├── SC-05_trivia_edit.html
│       ├── SC-10_video_list.html
│       ├── SC-11_video_create.html
│       ├── SC-12_video_edit.html
│       ├── SC-13_tag_list.html
│       ├── SC-14_tag_create.html
│       └── SC-15_tag_edit.html
│
├── src/
│   └── MyTrivia/
│       ├── Controllers/
│       │   ├── TriviaController.cs
│       │   ├── VideoController.cs
│       │   └── TagController.cs
│       ├── Models/
│       │   ├── Trivia.cs
│       │   ├── Tag.cs
│       │   ├── TriviaTag.cs
│       │   └── Video.cs
│       ├── Views/
│       │   ├── Trivia/
│       │   ├── Video/
│       │   ├── Tag/
│       │   └── Shared/
│       ├── Data/
│       │   └── AppDbContext.cs
│       ├── Migrations/
│       ├── wwwroot/
│       ├── appsettings.json
│       └── Program.cs
│
├── .gitignore
└── README.md
```

---

## 7. 注意事項・決定事項

- `appsettings.json` にSQLiteのファイルパスを記載するが、`.gitignore` で除外し `appsettings.Development.json` で管理する
- 動画削除時は紐づく雑学の `VideoId` をNULLにする（雑学自体は残す）
- タグ削除時は `TriviaTag` の該当レコードも同時に削除する（カスケード削除）
- クイズ機能は独立した画面・テーブルを持たず、`Trivia.QuizQuestion` カラムのみで管理する
- `Trivia.QuizQuestion` の正解は `Trivia.TriviaBody` を参照するため、正解カラムは持たない
