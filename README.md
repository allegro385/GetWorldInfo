VRChatのワールドポータルライブラリ用のjsonデータおよびサムネイル動画データを自動生成するC# (.NET8.0) プログラムです。
PortalLibrarySystem: [booth](https://booth.pm/ja/items/6659099)

---

## 必要環境
- .NET 8.0 SDK
- ffmpeg（PATHに追加）  
  インストール: [ffmpeg公式サイト](https://ffmpeg.org/)  
  インストール方法: [Qiita記事](https://qiita.com/Tadataka_Takahashi/items/9dcb0cf308db6f5dc31b) などを参照
---
## Excel入力レイアウト

| カテゴリ | ... | クエスト対応 | iOS対応 | ... | URL | ... | 個人メモ | ... |
| 00_ポータル | ... | 1 | 1 | ... | http://.../info | ... | 便利なポータルワールド | ... |

- **カテゴリ**: 頭3文字（例: `00_ホラー`, `01_イベント`）はソート用プレフィックスです
- **クエスト対応 / iOS対応**: 対応する場合は `1`、非対応は空欄
- **URL**: ワールドのURL
- **個人メモ**: 任意入力、jsonにも出力されます
- その他の項目は自動取得後にtsvに更新されます

---

## 入出力ファイル・運用サイクル

1. Excelで「ワールド一覧」を作成
2. 「**worlds.tsv**」として **01_Input** フォルダに保存（タブ区切り、1行目はヘッダー必須）
3. `WorldListGet.bat` を実行
4. `02_Output/yyyymmdd_hhmmss/` 以下に
   - `vrc-portal-world.json`（ポータル用json）
   - `portal-video.mp4`（サムネイル動画）
   - `worlds.tsv`（API自動取得反映済）
   が出力されます
5. 必要に応じてExcelにデータを再反映・修正し、以降繰り返し

---

## 注意点・トラブルシューティング

- ffmpegが動作しない場合は、インストールとPATH設定を確認してください
- 入力ファイル名やパスが違うと実行時エラーになります
- VRChat APIに接続できない場合はネットワークを見直してください
- `01_Input/worlds.tsv` 以外のファイルは参照されません

---
