// EmojiList.cs
using System.Collections.Generic;

public static class EmojiList
{
    public static List<string> Emojis { get; } = new List<string>
    {
        // Smileys & Emotion
        "😀", "😁", "😂", "🤣", "😃", "😄", "😅", "😆", "😉", "😊", "😋", "😎", "😍", "😘", "🥰", "😗",
        "😙", "😚", "🙂", "🤗", "🤩", "🤔", "🤨", "😐", "😑", "😶", "🙄", "😏", "😣", "😥", "😮", "🤐",
        "😯", "😪", "😫", "🥱", "😴", "😛", "😜", "🤪", "😝", "🤑", "🤭", "🤫", "🤥", "😒", "😓", "😔",
        "😕", "🙃", "🤑", "😲", "😷", "🤒", "🤕", "🤢", "🤮", "🤧", "🥵", "🥶", "🥳", "🤯", "😬", "😡", 
        "😠", "😤", "😭", "😱", "😨", "😰", "😥", "😢", "😓", "🤧", "🥺", "😞", "😔", "😟", "😕", "🙁",
        "😣", "😖", "😫", "😩", "🥱", "😤", "😡", "😠", "🤬", "😈", "👿", "💀", "☠️", "💩", "🤡", "👹", "👺",
        "👻", "👽", "👾", "🤖", "🎃", "😺", "😸", "😹", "😻", "😼", "😽", "🙀", "😿", "😾", 
        
        // People & Body
        "👶", "👧", "🧒", "👦", "👩", "🧑", "👨", "👩‍🦱", "👨‍🦱", "👩‍🦳", "👨‍🦳", "👩‍🦲", "👨‍🦲", "👩‍🦰", "👨‍🦰", 
        "👵", "🧓", "👴", "👲", "👳‍♀️", "👳‍♂️", "🧕", "👮‍♀️", "👮‍♂️", "👷‍♀️", "👷‍♂️", "💂‍♀️", "💂‍♂️", "🕵️‍♀️", 
        "🕵️‍♂️", "👩‍⚕️", "👨‍⚕️", "👩‍🌾", "👨‍🌾", "👩‍🍳", "👨‍🍳", "👩‍🎓", "👨‍🎓", "👩‍🎤", "👨‍🎤", "👩‍🏫", "👨‍🏫",
        "👩‍🏭", "👨‍🏭", "👩‍💻", "👨‍💻", "👩‍💼", "👨‍💼", "👩‍🔧", "👨‍🔧", "👩‍🔬", "👨‍🔬", "👩‍🎨", "👨‍🎨", "👩‍🚒",
        "👨‍🚒", "👩‍✈️", "👨‍✈️", "👩‍🚀", "👨‍🚀", "👩‍⚖️", "👨‍⚖️", "👰‍♀️", "🤵‍♂️", "👸", "🤴", "🦸‍♀️", "🦸‍♂️", 
        "🦹‍♀️", "🦹‍♂️", "🧙‍♀️", "🧙‍♂️", "🧛‍♀️", "🧛‍♂️", "🧟‍♀️", "🧟‍♂️", "🧞‍♀️", "🧞‍♂️", "🧜‍♀️", "🧜‍♂️",
        "🧝‍♀️", "🧝‍♂️", "🙍‍♀️", "🙍‍♂️", "🙎‍♀️", "🙎‍♂️", "🙅‍♀️", "🙅‍♂️", "🙆‍♀️", "🙆‍♂️", "💁‍♀️", "💁‍♂️",
        "🙋‍♀️", "🙋‍♂️", "🧏‍♀️", "🧏‍♂️", "🙇‍♀️", "🙇‍♂️", "🤦‍♀️", "🤦‍♂️", "🤷‍♀️", "🤷‍♂️", "💇‍♀️", "💇‍♂️", 
        "💆‍♀️", "💆‍♂️", "🧖‍♀️", "🧖‍♂️", "🧘‍♀️", "🧘‍♂️", "🛌", "🧑‍🤝‍🧑", "👭", "👫", "👬", "💏", "💑", "👪",
        
        // Animals & Nature
        "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼", "🐨", "🐯", "🦁", "🐮", "🐷", "🐽", "🐸", "🐵", 
        "🙈", "🙉", "🙊", "🐒", "🐔", "🐧", "🐦", "🐤", "🐣", "🐥", "🦆", "🦅", "🦉", "🦇", "🐺", "🐗", 
        "🐴", "🦄", "🐝", "🐛", "🦋", "🐌", "🐞", "🐜", "🦟", "🦗", "🐢", "🐍", "🦎", "🦂", "🦀", "🦞", 
        "🦑", "🐙", "🦐", "🐠", "🐟", "🐡", "🐬", "🐳", "🐋", "🦈", "🐊", "🐅", "🐆", "🦓", "🦍", "🦧", 
        "🐘", "🦛", "🦏", "🐪", "🐫", "🦒", "🦘", "🦥", "🦦", "🦨", "🦡", "🐁", "🐀", "🐇", "🦔", "🐿️",
        "🦢", "🦚", "🦜", "🦩", "🕊️", "🐉", "🐲", "🌵", "🎄", "🌲", "🌳", "🌴", "🌱", "🌿", "☘️", "🍀", 
        "🎍", "🎋", "🍃", "🍂", "🍁", "🍄", "🌾", "💐", "🌷", "🌹", "🥀", "🌺", "🌸", "🌼", "🌻", "🌞",
        
        // Food & Drink
        "🍇", "🍈", "🍉", "🍊", "🍋", "🍌", "🍍", "🥭", "🍎", "🍏", "🍐", "🍑", "🍒", "🍓", "🥝", "🍅",
        "🥥", "🥑", "🍆", "🥔", "🥕", "🌽", "🌶️", "🥒", "🥬", "🥦", "🧄", "🧅", "🍄", "🥜", "🌰", "🍞",
        "🥐", "🥖", "🥨", "🥯", "🥞", "🧇", "🧀", "🍖", "🍗", "🥩", "🥓", "🍔", "🍟", "🍕", "🌭", "🥪",
        "🌮", "🌯", "🥙", "🧆", "🥚", "🍳", "🥘", "🍲", "🥣", "🥗", "🍿", "🧈", "🧂", "🥫", "🍱", "🍘",
        "🍙", "🍚", "🍛", "🍜", "🍝", "🍠", "🍢", "🍣", "🍤", "🍥", "🥮", "🍡", "🥟", "🥠", "🥡", "🍦",
        "🍧", "🍨", "🍩", "🍪", "🎂", "🍰", "🧁", "🥧", "🍫", "🍬", "🍭", "🍮", "🍯", "🍼", "🥛", "☕",
        "🍵", "🍶", "🍾", "🍷", "🍸", "🍹", "🍺", "🍻", "🥂", "🥃", "🥤", "🧃", "🧉", "🧊", 
        
        // Travel & Places
        "🚗", "🚕", "🚙", "🚌", "🚎", "🏎️", "🚓", "🚑", "🚒", "🚐", "🚚", "🚛", "🚜", "🛴", "🚲", "🛵",
        "🏍️", "🛺", "🚨", "🚔", "🚍", "🚘", "🚖", "🚡", "🚠", "🚟", "🚃", "🚋", "🚞", "🚝", "🚄", "🚅",
        "🚈", "🚂", "🚆", "🚇", "🚊", "🚉", "✈️", "🛫", "🛬", "🛩️", "💺", "🛰️", "🚀", "🛸", "🚁", "🛶",
        "⛵", "🚤", "🛥️", "🛳️", "⛴️", "🚢", "⚓", "⛽", "🚧", "🚦", "🚥", "🛑", "🚏", "🗺️", "🗿", "🗽",
        "🗼", "🏰", "🏯", "🏟️", "🎡", "🎢", "🎠", "🏖️", "🏝️", "🏜️", "🌋", "🗻", "🏔️", "⛰️", "🏕️", 
        "🏠", "🏡", "🏘️", "🏚️", "🏢", "🏬", "🏭", "🏣", "🏤", "🏥", "🏦", "🏨", "🏪", "🏫", "🏩", "💒",
        
        // Symbols
        "❤️", "🧡", "💛", "💚", "💙", "💜", "🖤", "🤍", "🤎", "💔", "❣️", "💕", "💞", "💓", "💗", "💖", 
        "💘", "💝", "💟", "☮️", "✝️", "☪️", "🕉️", "☸️", "✡️", "🔯", "🕎", "☯️", "☦️", "🛐", "⛎", "♈", 
        "♉", "♊", "♋", "♌", "♍", "♎", "♏", "♐", "♑", "♒", "♓", "🆔", "⚛️", "🉑", "☢️", "☣️", "📴", 
        "📳", "🈶", "🈚", "🈸", "🈺", "🈷️", "✴️", "🆚", "🉐", "💮", "🉑", "㊗️", "㊙️", "🈁", "🔞", "🉑"
    };
}