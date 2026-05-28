# DB設計書 - 言語学雑学管理システム

作成日: 2024-01-01  
対象システム: ゆる言語学ラジオ 雑学・クイズ管理アプリ

---

## テーブル一覧

| テーブル名 | 説明 |
| --- | --- |
| Trivia | 雑学の本体 |
| Tag | タグマスタ |
| TriviaTag | 雑学とタグの中間テーブル |
| Video | YouTube動画マスタ |
| Quiz | クイズ |

---

## テーブル定義

### Trivia（雑学）

| カラム名 | データ型 | 制約 | NULL | 説明 |
|---|---|---|---|---|
| Id | INT | PK, AUTO_INCREMENT | NOT NULL | 雑学ID |
| TriviaTitle | NVARCHAR(200) | | NOT NULL | 雑学の見出し |
| TriviaBody | NVARCHAR(MAX) | | NOT NULL | 雑学の本文 |
| Source | NVARCHAR(200) | | NULL | 動画内の参照箇所メモ（例: 「12:34あたり」） |
| VideoId | INT | FK → Video.Id | NULL | 参照元の動画 |
| CreatedAt | DATETIME | | NOT NULL | 登録日時 |

### Tag（タグ）

| カラム名 | データ型 | 制約 | NULL | 説明 |
|---|---|---|---|---|
| Id | INT | PK, AUTO_INCREMENT | NOT NULL | タグID |
| TagName | NVARCHAR(50) | UNIQUE | NOT NULL | タグ名（例: ラテン語、和製英語） |

### TriviaTag（雑学タグ中間テーブル）

| カラム名 | データ型 | 制約 | NULL | 説明 |
|---|---|---|---|---|
| TriviaId | INT | PK, FK → Trivia.Id | NOT NULL | 雑学ID |
| TagId | INT | PK, FK → Tag.Id | NOT NULL | タグID |

> 複合主キー（TriviaId, TagId）で同じ組み合わせの重複を防ぐ。

### Video（YouTube動画）

| カラム名 | データ型 | 制約 | NULL | 説明 |
|---|---|---|---|---|
| Id | INT | PK, AUTO_INCREMENT | NOT NULL | 動画ID |
| VideoTitle | NVARCHAR(200) | | NOT NULL | 動画タイトル |
| VideoUrl | NVARCHAR(500) | UNIQUE | NOT NULL | YouTube URL |
| PublishedAt | DATE | | NULL | 動画の公開日 |

### Quiz（クイズ）

| カラム名 | データ型 | 制約 | NULL | 説明 |
|---|---|---|---|---|
| Id | INT | PK, AUTO_INCREMENT | NOT NULL | クイズID |
| TriviaId | INT | FK → Trivia.Id | NOT NULL | 対応する雑学ID |
| QuizQuestion | NVARCHAR(500) | | NOT NULL | 問題文 |
| QuizAnswer | NVARCHAR(200) | | NOT NULL | 正解 |
| QuizChoices | NVARCHAR(500) | | NOT NULL | 選択肢（カンマ区切り、正解を含む4択） |

---

## リレーション

```
Video ──< Trivia >── TriviaTag ──< Tag
              │
              └──< Quiz
```

- `Video` 1件に対して `Trivia` は複数紐づく（1本の動画に複数の雑学が登場する）
- `Trivia` 1件に対して `Tag` は複数紐づく（多対多、中間テーブル `TriviaTag` で管理）
- `Trivia` 1件に対して `Quiz` は0または1件（クイズ化していない雑学もある）
- `Trivia.VideoId` はNULL許容（動画不明の雑学も登録できる）

---

## サンプルデータ

### Video

| Id | VideoTitle | VideoUrl | PublishedAt |
|---|---|---|---|
| 1 | なぜ「勉強」は勉強というのか？語源を徹底解説 | https://youtube.com/watch?v=xxx1 | 2023-04-10 |
| 2 | カタカナ語の意外なルーツ【ゆる言語学ラジオ】 | https://youtube.com/watch?v=xxx2 | 2023-06-22 |

### Tag

| Id | TagName |
|---|---|
| 1 | ラテン語 |
| 2 | 漢語 |
| 3 | 英語由来 |
| 4 | 和製英語 |

### Trivia

| Id | TriviaTitle | TriviaBody | Source | VideoId | CreatedAt |
|---|---|---|---|---|---|
| 1 | 「勉強」はもともと無理やりやることの意味 | 「勉」は「無理にする」という意味の漢字で、江戸時代まではネガティブなニュアンスだった。商人が値引きする意味でも使われていた。 | 動画12:34あたり | 1 | 2024-01-05 |
| 2 | キャンペーンの語源は「平野」 | campaignはラテン語のcampus（平野）が語源。軍隊が平野で作戦を展開したことから「組織的な活動」を指すようになった。 | 動画8:20あたり | 2 | 2024-01-08 |

### TriviaTag

| TriviaId | TagId |
|---|---|
| 1 | 2 |
| 2 | 1 |

### Quiz

| Id | TriviaId | QuizQuestion | QuizAnswer | QuizChoices |
|---|---|---|---|---|
| 1 | 1 | 江戸時代、「勉強」はどんな意味で使われていた？ | 値引き・無理にすること | 値引き・無理にすること,学習,努力,修行 |
| 2 | 2 | 「キャンペーン」の語源となったラテン語の意味は？ | 平野 | 平野,戦争,旗,集会 |
