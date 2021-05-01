# MikuMikuMethods
C#向けMMD関連データ読込ライブラリ

## 構成
### /MikuMikuMethod
.NET Framework 4.8 (*Legacy*)
### /MikuMikuMethodCore
.NET 5 (*Recommended*)
### /Test
Framework版のテスト用プロジェクト
### /UnitTest
Core版の自動テスト

## 機能
Core版は開発中  
Framework版は開発終了

### Data I/O 

|Format|Core|Framework|
|:----:|:--:|:-------:|
|PMM|I/O|-|
|PMX|-|I/O|
|VMD|I/O|I/O|
|EMM|I/O|I/O|
|EMD|-|-|

Core版の未実装I/Oは今後製作予定あり  
(Pull request をくれてもいいのよ)

### Common
I/O以外の機能を放り込んでるフォルダ

- Encoding
  - .NET 5 でも簡単に Shift JIS を得るために用意
- Color
  - [0,1]の浮動小数点数で色情報を保持する構造体
- BoneNameComparer
  - ボーン名をいい感じにソートするための`IComparer<string>`  
    ソートする関数に引数として渡す
- Order
  - ByMap  
    `List<int>`の中身が新indexとなるようにリストを並び替える

## 今後の予定
先述の通り、すべての予定はCore版に関するもの

### PMX
内部表現は書いてるので、あとは入出力を書く
### EMD
EMMとそう変わらない  
内部表現は用意したので入出力を書けばどうにかなる
