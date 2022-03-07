# MikuMikuMethods
C#向けMMD関連データ読込ライブラリ

## 構成
### /MikuMikuMethod
ライブラリ本体
### /UnitTest
自動テスト

## 機能
### Data I/O 
以下のファイルフォーマットに対応

- PMM
- PMX
- VMD
- EMM
- EMD

### Common
I/O以外の機能を放り込んでるフォルダ

- Encoding
  - .NET でも簡単に Shift JIS を得るために用意
- Color
  - 浮動小数点数で色情報を保持する構造体
- BoneNameComparer
  - ボーン名をいい感じにソートするための`IComparer<string>`  
    ソートする関数に引数として渡す
- Order
  - ByMap  
    `List<int>`の中身が新indexとなるようにリストを並び替える
