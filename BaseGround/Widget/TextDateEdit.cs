using Common;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.ComponentModel;
using System.Drawing;

namespace BaseGround.Widget {
   [UserRepositoryItem("RegisterTextDateEdit")]
   public class RepositoryItemTextDateEdit : RepositoryItemTextEdit {
      static RepositoryItemTextDateEdit() {
         RegisterMaskDateEdit();
      }

      public const string CustomEditName = "TextDateEdit";

      public RepositoryItemTextDateEdit() {
         Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
         Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
         Mask.UseMaskAsDisplayFormat = true;
         Mask.ShowPlaceHolders = false;
         Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
      }

      public override string EditorTypeName => CustomEditName;

      public static void RegisterMaskDateEdit() {
         Image img = null;
         EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(CustomEditName, typeof(TextDateEdit), typeof(RepositoryItemTextDateEdit), typeof(MaskDateEditViewInfo), new MaskDateEditPainter(), true, img));
      }

      public override void Assign(RepositoryItem item) {
         BeginUpdate();
         try {
            base.Assign(item);
            RepositoryItemTextDateEdit source = item as RepositoryItemTextDateEdit;
            if (source == null) return;
            //
         } finally {
            EndUpdate();
         }
      }
   }

   [ToolboxItem(true)]
   public class TextDateEdit : TextEdit {
      static TextDateEdit() {
         RepositoryItemTextDateEdit.RegisterMaskDateEdit();
      }

      public TextDateEdit() {
         //_TextFormat = "yyyy/MM/dd";
         //_DateType = DateTypeItem.Date;
      }

      [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
      public new RepositoryItemTextDateEdit Properties => base.Properties as RepositoryItemTextDateEdit;

      public override string EditorTypeName => RepositoryItemTextDateEdit.CustomEditName;

      private DateTypeItem _DateType;

      public DateTypeItem DateType {
         get => _DateType; set {
            _DateType = value;
            switch (value) {
               case DateTypeItem.Date:
                  Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
                  Properties.MaxLength = 0;
                  Properties.Mask.ShowPlaceHolders = false;
                  _TextFormat = "yyyy/MM/dd";
                  break;

               case DateTypeItem.Month:
                  Properties.Mask.EditMask = "[1-9]\\d{3}/(0[1-9]|1[0-2])";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
                  Properties.MaxLength = 0;
                  Properties.Mask.ShowPlaceHolders = false;
                  _TextFormat = "yyyy/MM";
                  break;

               case DateTypeItem.Year:
                  Properties.Mask.EditMask = "[1-9]\\d{3}";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
                  Properties.MaxLength = 4;
                  Properties.Mask.ShowPlaceHolders = false;
                  _TextFormat = "yyyy";
                  break;
                  //case DateTypeItem.Time:
                  //   Properties.Mask.EditMask = "HH:mm:ss";
                  //   Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
                  //   Properties.MaxLength = 0;
                  //   break;
            }
         }
      }

      private string _TextFormat;

      private TextMaskFormatItem _TextMaskFormat;

      public TextMaskFormatItem TextMaskFormat {
         get => _TextMaskFormat;
         set {
            _TextMaskFormat = value;
            switch (value) {
               case TextMaskFormatItem.IncludePrompt:
                  Properties.EditFormat.FormatString = _TextFormat.Replace(@"/", "").Replace(":", "");
                  //_TextFormat = Properties.Mask.EditMask.Replace(@"/", "").Replace(":", "");
                  break;

               case TextMaskFormatItem.IncludePromptAndLiterals:
                  Properties.EditFormat.FormatString = _TextFormat;
                  break;
            }
         }
      }

      public string FormatValue {
         get {
            if (DateType == DateTypeItem.Year) {
               return Text;
            } else {
               return string.IsNullOrEmpty(Text) ? "" : Convert.ToDateTime(Text).ToString(Properties.EditFormat.FormatString);
            }
         }
      }

      private DateTime _DateTimeValue;

      public DateTime DateTimeValue {
         get {
            //string text = Text;
            //if (DateType == DateTypeItem.Year) {
            //   text = text + "/01/01";
            //} else if (DateType == DateTypeItem.Month) {
            //   text = text + "/01";
            //}
            return Text.AsDateTime(_TextFormat);//string.IsNullOrEmpty(Text) ? new DateTime() : text.AsDateTime("yyyy/MM/dd");
         }
         set {
            _DateTimeValue = value;
            //if (DateType == DateTypeItem.Year) {
            //   Text = _DateTimeValue.ToString("yyyy");
            //} else {
            Text = _DateTimeValue.ToString(_TextFormat);
            //}
         }
      }

      public enum TextMaskFormatItem {
         IncludePromptAndLiterals,
         IncludePrompt
      };

      public enum DateTypeItem {
         Date,
         Month,
         Year,
         Time
      };

      protected override bool IsMatch => 
         Text.AsDateTime(_TextFormat) == DateTime.MinValue ? false : true;


      protected override void DoIsMatchValidating(CancelEventArgs e) {
         if (!IsMatch) {
            MessageDisplay.Info("日期輸入錯誤 !");
            Focus();
            DefaultErrorImageOptions.Image = new Bitmap(1,1);
            e.Cancel = true;
         }
      }
   }

   public class MaskDateEditViewInfo : TextEditViewInfo {
      public MaskDateEditViewInfo(RepositoryItem item) : base(item) {
      }
   }

   public class MaskDateEditPainter : TextEditPainter {
      public MaskDateEditPainter() {
      }
   }
}