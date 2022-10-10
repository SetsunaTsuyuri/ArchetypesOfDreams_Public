# 夢幻のアルカイク

## 紹介動画
https://www.youtube.com/watch?v=TBNxfm1hqPU

## アピールポイント
### 有限ステートマシンの自作
汎用的な有限ステートマシンを自作し、戦闘処理にこれを活用しました。
ターン開始・キャラクターの行動等の各フェイズをステートに分けることで、コードから戦いの流れが把握し易くなりました。

### UniTaskを活用した非同期処理
Unityのコルーチンを使わず、UniTaskを用いて非同期処理を実装しました。
MonoBehaviourに依存する問題を解消しました。

### SerializeReferenceの活用
基底クラスやインターフェースをシリアライズ可能にする機能です。
セーブデータの味方パーティ情報をjsonに書き出し保存する際に活用しました。
パーティの情報を「人間」と「モンスター」の基底クラスとなる「戦闘者クラス」の配列とすることで、
それらの混成パーティをシリアライズ(=データの保存)可能にしました。

### TiledMapEditorの活用
このゲームのダンジョンは、TiledMapEditorというフリーのエディタツールでマップデータを作り、そこから出力したjsonファイルを読み込んで動的にマップを生成する仕組みになっています。
マップの形状のみならず、そこで起きるイベント(戦闘や会話等)もTiledMapで設定できます。
これにより、Unityのライセンスがなくてもマップのレベルデザインできます。
