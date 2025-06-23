# プレイヤーコントローラー設定ガイド（DonkeyPlayerController 用）

このガイドは、`DonkeyPlayerController` スクリプトを正しく動作させるための Unity エディタ上の設定手順を記載しています。

---
    　
## ✅ GroundCheck の設定手順

`DonkeyPlayerController` は、ジャンプ可能かを判定するために `groundCheck` という `Transform` を使用します。これはプレイヤーキャラクターの足元に置く必要があります。

### 手順：

1. プレイヤーオブジェクトを選択（スクリプトがアタッチされているオブジェクト）
2. **右クリック → Create Empty** で子オブジェクトを作成し、名前を `GroundCheck` に変更
3. Transform の `Position` を `(0, -1, 0)` 程度に設定（キャラの足元あたり）
4. 親であるプレイヤーの Inspector にある `DonkeyPlayerController` スクリプトの `groundCheck` フィールドに、この `GroundCheck` をドラッグ＆ドロップで割り当て

---

## ✅ Ground Layer の設定手順

プレイヤーが「接地しているかどうか」を判定するには、地面オブジェクトが特定のレイヤーに設定されている必要があります。

### 手順：

1. Unity 上部メニューの `Layer` → `Add Layer...` を選択
2. 空いているレイヤースロットに `Ground` という名前を追加
3. 地面（Tilemap や Platform など）を選択し、Inspector の `Layer` を `Ground` に設定
4. プレイヤーの `DonkeyPlayerController` スクリプトの `groundLayer` に `Ground` を指定（レイヤーマスクで選択）

---

## ✅ 推奨パラメータ設定

| 項目                  | 推奨値           | 内容                 |
| ------------------- | ------------- | ------------------ |
| `groundCheckRadius` | `0.2` ～ `0.3` | 小さくして足元ピンポイントに接地判定 |
| `groundCheck`       | `GroundCheck` | 足元に設置した空オブジェクト     |
| `groundLayer`       | `Ground`      | 地面とするレイヤー          |

---

## ✅ テスト方法

* ゲーム開始後、`Z` キーでローリングが発動し、接地時にのみ使用可能であること
* `Space` キーでジャンプができ、空中では `groundCheck` によって `isGrounded == false` になること
* 空中では移動慣性が残っているが、やや鈍くなる（滑空感あり）

---

これらの手順を満たすことで、スクリプトが期待通りに動作するようになります。
カメラ追従、アニメーション、エフェクトなどを追加する場合は、別途設定が必要です。


