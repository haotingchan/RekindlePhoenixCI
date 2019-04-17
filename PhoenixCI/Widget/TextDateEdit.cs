using Common;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using System;
using System.ComponentModel;
using System.Drawing;

namespace PhoenixCI.Widget {
   [UserRepositoryItem("RegisterTextDateEdit")]
   public class RepositoryItemTextDateEdit : RepositoryItemTextEdit {
      static RepositoryItemTextDateEdit() {
         RegisterMaskDateEdit();
      }

      public const string CustomEditName = "TextDateEdit";

      public RepositoryItemTextDateEdit() {
         Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
         Mask.EditMask = "yyyy/MM/dd";
         Mask.UseMaskAsDisplayFormat = true;
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
         _TextFormat = "yyyy/MM/dd";
         _DateType = DateTypeItem.Date;
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
                  Properties.Mask.EditMask = "yyyy/MM/dd";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
                  Properties.MaxLength = 0;
                  break;

               case DateTypeItem.Month:
                  Properties.Mask.EditMask = "yyyy/MM";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
                  Properties.MaxLength = 0;
                  break;

               case DateTypeItem.Year:
                  Properties.Mask.EditMask = "0000";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
                  Properties.Mask.PlaceHolder = '0';
                  Properties.MaxLength = 4;
                  break;
               case DateTypeItem.Time:
                  Properties.Mask.EditMask = "HH:mm:ss";
                  Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
                  Properties.MaxLength = 0;
                  break;
            }
            _TextFormat = Properties.Mask.EditMask;
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
                  _TextFormat = Properties.Mask.EditMask.Replace(@"/", "").Replace(":", "");
                  break;

               case TextMaskFormatItem.IncludePromptAndLiterals:
                  _TextFormat = Properties.Mask.EditMask;
                  break;
            }
         }
      }

      public string FormatValue {
         get {
            if (DateType == DateTypeItem.Year) {
               return Text;
            } else {
               return string.IsNullOrEmpty(Text) ? "" : Convert.ToDateTime(Text).ToString(_TextFormat);
            }
         }
      }

      private DateTime _DateTimeValue;

      public DateTime DateTimeValue {
         get {
            string text = Text;
            if (DateType == DateTypeItem.Year) {
               text = text + "/01/01";
            } else if (DateType == DateTypeItem.Month) {
               text = text + "/01";
            }
            return text.AsDateTime(Properties.Mask.EditMask);//string.IsNullOrEmpty(Text) ? new DateTime() : text.AsDateTime("yyyy/MM/dd");
         }
         set {
            _DateTimeValue = value;
            if (DateType == DateTypeItem.Year) {
               Text = _DateTimeValue.ToString("yyyy");
            } else {
               Text = _DateTimeValue.ToString(Properties.Mask.EditMask);
            }
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