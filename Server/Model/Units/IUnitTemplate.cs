using System.Drawing;

namespace GameServer.Model.Units
{
    /// <summary>
    /// Contains template ids to generate a unit
    /// </summary>
    public interface IUnitTemplate
    {
        string Name                     { get; }
        uint Id                         { get; }
        uint Team                       { get; }
        
        uint HeadTemplateId             { get; }
        uint ChestTemplateId            { get; }
        uint ArmsTemplateId             { get; }
        uint LegsTemplateId             { get; }
        uint BackpackTemplateId         { get; }
        
        Color HeadColor                 { get; }
        Color ChestColor                { get; }
        Color ArmsColor                 { get; }
        Color LegsColor                 { get; }
        Color BackpackColor             { get; }
        
        uint Weapon1LeftTemplateId      { get; }
        uint Weapon1RightTemplateId     { get; }
        
        uint Weapon2LeftTemplateId      { get; }
        uint Weapon2RightTemplateId     { get; }
        
        uint Skill1TemplateId           { get; }
        uint Skill2TemplateId           { get; }
        uint Skill3TemplateId           { get; }
        uint Skill4TemplateId           { get; }
    }
}