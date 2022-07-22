using System;
using System.Collections.Generic;
using System.Linq;
using DotOPDS.Contract;

namespace DotOPDS.BookProvider.Inpx;

public class Genres
{
    public Genres(ITranslator translator)
    {
        T = translator;
        InitLocalization();
        InitTree();
    }

    public List<Tuple<string, string>> Localize(string genre)
    {
        var categories = Tree.AsParallel().Where(k => k.Value.Contains(genre)).Select(x => x.Key);
        var gen = localized.ContainsKey(genre) ? localized[genre] : genre;
        var ret = new List<Tuple<string, string>>();
        foreach (var cat in categories)
        {
            var category = localized.ContainsKey(cat) ? localized[cat] : cat;
            ret.Add(new Tuple<string, string>(category, gen));
        }
        return ret;
    }

    //  ([a-z_]+) ([\S ]+)
    // localized.Add("\1", T._("\2"));
    private readonly Dictionary<string, string> localized = new();
    private readonly Dictionary<string, List<string>> Tree = new();
    private readonly ITranslator T;

    private void InitLocalization()
    {
        localized.Add("accounting", T._("Accounting"));
        localized.Add("adv_animal", T._("Nature & Animals"));
        localized.Add("adv_geo", T._("Travel & Geography"));
        localized.Add("adv_history", T._("Historical adventures"));
        localized.Add("adv_indian", T._("Indians"));
        localized.Add("adv_maritime", T._("Maritime Fiction"));
        localized.Add("adv_western", T._("Western"));
        localized.Add("adventure", T._("Misk Adventures"));
        localized.Add("antique", T._("Other Antique"));
        localized.Add("antique_ant", T._("Antique Literature"));
        localized.Add("antique_east", T._("Antique East Literature"));
        localized.Add("antique_european", T._("Antique European Literature"));
        localized.Add("antique_myths", T._("Myths. Legends. Epos"));
        localized.Add("antique_russian", T._("Antique Russian Literature"));
        localized.Add("aphorisms", T._("Aphorisms"));
        localized.Add("architecture_book", T._("Architecture"));
        localized.Add("astrology", T._("Astrology"));
        localized.Add("auto_regulations", T._("Auto regulations"));
        localized.Add("banking", T._("Banking"));
        localized.Add("child_adv", T._("Adventures for Kids"));
        localized.Add("child_det", T._("Detective for Kids"));
        localized.Add("child_education", T._("Education for Kids"));
        localized.Add("child_folklore", T._("Child Folklore"));
        localized.Add("child_prose", T._("Prose for Kids"));
        localized.Add("child_sf", T._("Science Fiction for Kids"));
        localized.Add("child_tale", T._("Fairy Tales"));
        localized.Add("child_verse", T._("Verses for Kids"));
        localized.Add("children", T._("For Kids: Misk"));
        localized.Add("cine", T._("Cine"));
        localized.Add("comedy", T._("Comedy"));
        localized.Add("comp_db", T._("Databases"));
        localized.Add("comp_dsp", T._("DSP"));
        localized.Add("comp_hard", T._("Hardware"));
        localized.Add("comp_osnet", T._("OS & Networking"));
        localized.Add("comp_programming", T._("Programming"));
        localized.Add("comp_soft", T._("Software"));
        localized.Add("comp_www", T._("Internet"));
        localized.Add("computers", T._("Computers: Misk"));
        localized.Add("design", T._("Art, Design"));
        localized.Add("det_action", T._("Action"));
        localized.Add("det_classic", T._("Classical Detective"));
        localized.Add("det_cozy", T._("Cozy Mysteries"));
        localized.Add("det_crime", T._("Crime Detective"));
        localized.Add("det_espionage", T._("Espionage Detective"));
        localized.Add("det_hard", T._("Hard-boiled Detective"));
        localized.Add("det_history", T._("Historical Detective"));
        localized.Add("det_irony", T._("Ironical Detective"));
        localized.Add("det_maniac", T._("Maniacs"));
        localized.Add("det_police", T._("Police Stories"));
        localized.Add("det_political", T._("Political Detective"));
        localized.Add("detective", T._("Detective"));
        localized.Add("dissident", T._("Anti-Soviet fiction"));
        localized.Add("drama", T._("Drama"));
        localized.Add("dramaturgy", T._("Dramaturgy"));
        localized.Add("economics", T._("Economics"));
        localized.Add("epic", T._("Epic"));
        localized.Add("epic_poetry", T._("Epic Poetry"));
        localized.Add("epistolary_fiction", T._("Epistolary"));
        localized.Add("essay", T._("Essay"));
        localized.Add("experimental_poetry", T._("Experimental Poetry"));
        localized.Add("extravaganza", T._("Extravaganza"));
        localized.Add("fable", T._("Fable"));
        localized.Add("fairy_fantasy", T._("Fairy SF"));
        localized.Add("fanfiction", T._("Fan fiction"));
        localized.Add("folk_songs", T._("Folk Songs"));
        localized.Add("folk_tale", T._("Folk tales"));
        localized.Add("folklore", T._("Folklore"));
        localized.Add("foreign_language", T._("Foreign languages"));
        localized.Add("geo_guides", T._("Guides"));
        localized.Add("global_economy", T._("Global Economy"));
        localized.Add("gothic_novel", T._("Gothic novel"));
        localized.Add("great_story", T._("Great story"));
        localized.Add("historical_fantasy", T._("Historical fantasy"));
        localized.Add("home", T._("Home: Other"));
        localized.Add("home_collecting", T._("Collecting"));
        localized.Add("home_cooking", T._("Cooking"));
        localized.Add("home_crafts", T._("Hobbies & Crafts"));
        localized.Add("home_diy", T._("Do it yourself"));
        localized.Add("home_entertain", T._("Entertaining"));
        localized.Add("home_garden", T._("Garden"));
        localized.Add("home_health", T._("Health"));
        localized.Add("home_pets", T._("Pets"));
        localized.Add("home_sex", T._("Erotica, Sex"));
        localized.Add("home_sport", T._("Sports"));
        localized.Add("humor", T._("Misk Humor"));
        localized.Add("humor_anecdote", T._("Anecdote"));
        localized.Add("humor_fantasy", T._("Humorous fantasy"));
        localized.Add("humor_prose", T._("Humor Prose"));
        localized.Add("humor_satire", T._("Satire"));
        localized.Add("humor_verse", T._("Humor Verses"));
        localized.Add("in_verse", T._("In verse"));
        localized.Add("industries", T._("Industries"));
        localized.Add("job_hunting", T._("Job Hunting"));
        localized.Add("limerick", T._("Limerick"));
        localized.Add("love", T._("About love"));
        localized.Add("love_contemporary", T._("Contemporary Romance"));
        localized.Add("love_detective", T._("Detective Romance"));
        localized.Add("love_erotica", T._("Erotica"));
        localized.Add("love_hard", T._("Porno"));
        localized.Add("love_history", T._("Historical Romance"));
        localized.Add("love_sf", T._("Love SF"));
        localized.Add("love_short", T._("Short Romance"));
        localized.Add("lyrics", T._("Lyrics"));
        localized.Add("management", T._("Management"));
        localized.Add("marketing", T._("Marketing, PR, Adv"));
        localized.Add("military", T._("Military"));
        localized.Add("military_arts", T._("Military Arts"));
        localized.Add("military_history", T._("Military History"));
        localized.Add("military_special", T._("Military special"));
        localized.Add("military_weapon", T._("Weapon"));
        localized.Add("music", T._("Music"));
        localized.Add("mystery", T._("Mystery"));
        localized.Add("nonf_biography", T._("Biography & Memoirs"));
        localized.Add("nonf_criticism", T._("Criticism"));
        localized.Add("nonf_military", T._("Military docs"));
        localized.Add("nonf_publicism", T._("Publicism"));
        localized.Add("nonfiction", T._("Misk Nonfiction"));
        localized.Add("notes", T._("Notes"));
        localized.Add("nsf", T._("Non Science Fiction"));
        localized.Add("org_behavior", T._("Corporate Culture"));
        localized.Add("other", T._("Other"));
        localized.Add("palindromes", T._("Palindromes"));
        localized.Add("palmistry", T._("Palmistry"));
        localized.Add("paper_work", T._("Paper Work"));
        localized.Add("periodic", T._("Periodic"));
        localized.Add("personal_finance", T._("Personal Finance"));
        localized.Add("poetry", T._("Poetry"));
        localized.Add("popadanec", T._("Trappee"));
        localized.Add("popular_business", T._("Popular Business"));
        localized.Add("prose", T._("Prose"));
        localized.Add("prose_classic", T._("Classical Prose"));
        localized.Add("prose_contemporary", T._("Contemporary Prose"));
        localized.Add("prose_counter", T._("Counterculture"));
        localized.Add("prose_epic", T._("Epopee"));
        localized.Add("prose_game", T._("Game book"));
        localized.Add("prose_history", T._("Historical Prose"));
        localized.Add("prose_magic", T._("Magic realism"));
        localized.Add("prose_military", T._("Military Prose"));
        localized.Add("prose_rus_classic", T._("Russian Classics"));
        localized.Add("prose_sentimental", T._("Sentimental Prose"));
        localized.Add("prose_su_classics", T._("Soviet Classics"));
        localized.Add("proverbs", T._("Proverbs"));
        localized.Add("psy_childs", T._("Child Psychology"));
        localized.Add("psy_sex_and_family", T._("Sex and family"));
        localized.Add("psy_theraphy", T._("Psychotherapy and counselling"));
        localized.Add("real_estate", T._("Real Estate"));
        localized.Add("ref_dict", T._("Dictionaries"));
        localized.Add("ref_encyc", T._("Encyclopedias"));
        localized.Add("ref_guide", T._("Guidebooks"));
        localized.Add("ref_ref", T._("Reference"));
        localized.Add("reference", T._("Misk References"));
        localized.Add("religion", T._("Religion: Other"));
        localized.Add("religion_budda", T._("Buddha"));
        localized.Add("religion_catholicism", T._("Catholicism"));
        localized.Add("religion_christianity", T._("Christianity"));
        localized.Add("religion_esoterics", T._("Esoterics"));
        localized.Add("religion_hinduism", T._("Hinduism"));
        localized.Add("religion_islam", T._("Islam"));
        localized.Add("religion_judaism", T._("Judaism"));
        localized.Add("religion_orthodoxy", T._("Orthodoxy"));
        localized.Add("religion_paganism", T._("Paganism"));
        localized.Add("religion_protestantism", T._("Protestantism"));
        localized.Add("religion_rel", T._("Religion"));
        localized.Add("religion_self", T._("Self-perfection"));
        localized.Add("riddles", T._("Riddles"));
        localized.Add("roman", T._("Roman"));
        localized.Add("sagas", T._("Sagas"));
        localized.Add("scenarios", T._("Scenarios"));
        localized.Add("sci_abstract", T._("Abstract"));
        localized.Add("sci_anachem", T._("Analitic Chemistry"));
        localized.Add("sci_biochem", T._("Biochemistry"));
        localized.Add("sci_biology", T._("Biology"));
        localized.Add("sci_biophys", T._("Biophysics"));
        localized.Add("sci_botany", T._("Botany"));
        localized.Add("sci_build", T._("Building"));
        localized.Add("sci_business", T._("Business"));
        localized.Add("sci_chem", T._("Chemistry"));
        localized.Add("sci_cosmos", T._("Cosmos"));
        localized.Add("sci_crib", T._("Cribs"));
        localized.Add("sci_culture", T._("Cultural Science"));
        localized.Add("sci_ecology", T._("Ecology"));
        localized.Add("sci_economy", T._("Economy"));
        localized.Add("sci_geo", T._("Geology"));
        localized.Add("sci_history", T._("History"));
        localized.Add("sci_juris", T._("Jurisprudence"));
        localized.Add("sci_linguistic", T._("Linguistics"));
        localized.Add("sci_math", T._("Mathematics"));
        localized.Add("sci_medicine", T._("Medicine"));
        localized.Add("sci_medicine_alternative", T._("Alternative medicine"));
        localized.Add("sci_metal", T._("Metallurgy"));
        localized.Add("sci_orgchem", T._("Organic Chemistry"));
        localized.Add("sci_pedagogy", T._("Pedagogy"));
        localized.Add("sci_philology", T._("Philology"));
        localized.Add("sci_philosophy", T._("Philosophy"));
        localized.Add("sci_phys", T._("Physics"));
        localized.Add("sci_physchem", T._("Physical chemistry"));
        localized.Add("sci_politics", T._("Politics"));
        localized.Add("sci_popular", T._("Science popular"));
        localized.Add("sci_psychology", T._("Psychology"));
        localized.Add("sci_radio", T._("Radio"));
        localized.Add("sci_religion", T._("Religious Studies"));
        localized.Add("sci_social_studies", T._("Social studies"));
        localized.Add("sci_state", T._("State science"));
        localized.Add("sci_tech", T._("Technical"));
        localized.Add("sci_textbook", T._("Textbook"));
        localized.Add("sci_transport", T._("Transport"));
        localized.Add("sci_veterinary", T._("Veterinary"));
        localized.Add("sci_zoo", T._("Zoology"));
        localized.Add("science", T._("Misk Science, Education"));
        localized.Add("screenplays", T._("Screenplays"));
        localized.Add("sf", T._("Science Fiction"));
        localized.Add("sf_action", T._("Action SF"));
        localized.Add("sf_cyberpunk", T._("Cyberpunk"));
        localized.Add("sf_detective", T._("Detective SF"));
        localized.Add("sf_epic", T._("Epic SF"));
        localized.Add("sf_etc", T._("Other SF"));
        localized.Add("sf_fantasy", T._("Fantasy"));
        localized.Add("sf_fantasy_city", T._("Fantasy city"));
        localized.Add("sf_fantasy_irony", T._("Ironyc female fantasy"));
        localized.Add("sf_heroic", T._("Heroic SF"));
        localized.Add("sf_history", T._("Alternative history"));
        localized.Add("sf_horror", T._("Horror"));
        localized.Add("sf_humor", T._("Humor SF"));
        localized.Add("sf_irony", T._("Ironical SF"));
        localized.Add("sf_mystic", T._("Mystic"));
        localized.Add("sf_postapocalyptic", T._("Postapocalyptic"));
        localized.Add("sf_social", T._("Social SF"));
        localized.Add("sf_space", T._("Space SF"));
        localized.Add("sf_space_opera", T._("Space Opera"));
        localized.Add("sf_stimpank", T._("Stimpank"));
        localized.Add("sf_technofantasy", T._("Technofantasy"));
        localized.Add("short_story", T._("Short story"));
        localized.Add("small_business", T._("Small Business"));
        localized.Add("song_poetry", T._("Song Poetry"));
        localized.Add("stock", T._("Stock"));
        localized.Add("story", T._("Story"));
        localized.Add("theatre", T._("Theatre"));
        localized.Add("thriller", T._("Thrillers"));
        localized.Add("thriller_legal", T._("Legal thriller"));
        localized.Add("thriller_medical", T._("Medical thriller"));
        localized.Add("thriller_techno", T._("Techno Thriller"));
        localized.Add("trade", T._("Trade"));
        localized.Add("tragedy", T._("Tragedy"));
        localized.Add("unfinished", T._("Unfinished"));
        localized.Add("vaudeville", T._("Vaudeville"));
        localized.Add("vers_libre", T._("Vers libre"));
        localized.Add("visual_arts", T._("Visual Arts"));
        localized.Add("visual_poetry", T._("Visual Poetry"));
        localized.Add("ya", T._("Young-adult fiction"));

        localized.Add("category_fantasy", T._p("Genres|Category|", "SF, Fantasy"));
        localized.Add("category_detective", T._p("Genres|Category|", "Detectives, Thrillers"));
        localized.Add("category_prose", T._p("Genres|Category|", "Prose"));
        localized.Add("category_love", T._p("Genres|Category|", "Love"));
        localized.Add("category_adventures", T._p("Genres|Category|", "Adventures"));
        localized.Add("category_child", T._p("Genres|Category|", "Children's"));
        localized.Add("category_poetry", T._p("Genres|Category|", "Poetry, Dramaturgy"));
        localized.Add("category_antique", T._p("Genres|Category|", "Antique"));
        localized.Add("category_science", T._p("Genres|Category|", "Science, Education"));
        localized.Add("category_comp", T._p("Genres|Category|", "Computers"));
        localized.Add("category_ref", T._p("Genres|Category|", "Reference"));
        localized.Add("category_religion", T._p("Genres|Category|", "Religion"));
        localized.Add("category_humor", T._p("Genres|Category|", "Humor"));
        localized.Add("category_home", T._p("Genres|Category|", "Home, Family"));
        localized.Add("category_technics", T._p("Genres|Category|", "Technics"));
        localized.Add("category_other", T._p("Genres|Category|", "Other"));
        localized.Add("category_business", T._p("Genres|Category|", "Economy, Business"));
        localized.Add("category_nonfiction", T._p("Genres|Category|", "Nonfiction"));
        localized.Add("category_drama", T._p("Genres|Category|", "Dramaturgy"));
        localized.Add("category_folk", T._p("Genres|Category|", "Folklore"));
        localized.Add("category_military", T._p("Genres|Category|", "Military"));
    }

    //  ([a-z_]+) ([\S ]+)
    // list.Add("\1");
    private void InitTree()
    {
        List<string> list;

        list = new List<string>
        {
            "sf_history",
            "sf_action",
            "sf_epic",
            "sf_heroic",
            "sf_detective",
            "sf_cyberpunk",
            "sf_space",
            "sf_social",
            "sf_horror",
            "sf_humor",
            "sf_fantasy",
            "sf",
            "child_sf",
            "sf_fantasy_city",
            "sf_postapocalyptic",
            "love_sf",
            "gothic_novel",
            "nsf",
            "fairy_fantasy",
            "sf_etc",
            "sf_irony",
            "sf_fantasy_irony",
            "sf_mystic",
            "sf_space_opera",
            "sf_stimpank",
            "sf_technofantasy",
            "popadanec",
            "historical_fantasy",
            "humor_fantasy"
        };
        Tree.Add("category_fantasy", list);

        list = new List<string>
        {
            "sf_detective",
            "det_classic",
            "det_police",
            "det_action",
            "det_irony",
            "det_history",
            "det_espionage",
            "det_crime",
            "det_political",
            "det_maniac",
            "det_hard",
            "thriller",
            "detective",
            "love_detective",
            "child_det",
            "thriller_legal",
            "thriller_medical",
            "thriller_techno",
            "det_cozy"
        };
        Tree.Add("category_detective", list);

        list = new List<string>
        {
            "prose",
            "prose_classic",
            "prose_history",
            "prose_contemporary",
            "prose_counter",
            "prose_rus_classic",
            "prose_su_classics",
            "prose_military",
            "aphorisms",
            "essay",
            "story",
            "great_story",
            "short_story",
            "roman",
            "extravaganza",
            "epistolary_fiction",
            "prose_epic",
            "prose_magic",
            "sagas",
            "dissident",
            "prose_sentimental"
        };
        Tree.Add("category_prose", list);

        list = new List<string>
        {
            "love_contemporary",
            "love_history",
            "love_detective",
            "love_short",
            "love_erotica",
            "love",
            "love_sf",
            "love_hard",
            "det_cozy"
        };
        Tree.Add("category_love", list);

        list = new List<string>
        {
            "adv_western",
            "adv_history",
            "adv_indian",
            "adv_maritime",
            "adv_geo",
            "adv_animal",
            "adventure",
            "child_adv"
        };
        Tree.Add("category_adventures", list);

        list = new List<string>
        {
            "child_tale",
            "child_verse",
            "child_prose",
            "child_sf",
            "child_det",
            "child_adv",
            "child_education",
            "children",
            "child_folklore",
            "prose_game"
        };
        Tree.Add("category_child", list);

        list = new List<string>
        {
            "child_verse",
            "poetry",
            "humor_verse",
            "fable",
            "vers_libre",
            "visual_poetry",
            "lyrics",
            "palindromes",
            "song_poetry",
            "experimental_poetry",
            "epic_poetry",
            "in_verse"
        };
        Tree.Add("category_poetry", list);

        list = new List<string>
        {
            "antique_ant",
            "antique_european",
            "antique_russian",
            "antique_east",
            "antique_myths",
            "antique"
        };
        Tree.Add("category_antique", list);

        list = new List<string>
        {
            "sci_history",
            "sci_psychology",
            "sci_culture",
            "sci_religion",
            "sci_philosophy",
            "sci_politics",
            "sci_business",
            "sci_juris",
            "sci_linguistic",
            "sci_medicine",
            "sci_phys",
            "sci_math",
            "sci_chem",
            "sci_biology",
            "sci_tech",
            "science",
            "sci_biochem",
            "sci_physchem",
            "sci_anachem",
            "sci_orgchem",
            "sci_economy",
            "sci_state",
            "sci_biophys",
            "sci_geo",
            "sci_cosmos",
            "sci_medicine_alternative",
            "sci_philology",
            "sci_pedagogy",
            "sci_social_studies",
            "sci_ecology",
            "military_history",
            "sci_veterinary",
            "sci_zoo",
            "sci_botany",
            "sci_textbook",
            "sci_crib",
            "sci_abstract",
            "foreign_language",
            "psy_childs",
            "psy_theraphy",
            "psy_sex_and_family"
        };
        Tree.Add("category_science", list);

        list = new List<string>
        {
            "comp_www",
            "comp_programming",
            "comp_hard",
            "comp_soft",
            "comp_db",
            "comp_osnet",
            "computers",
            "comp_dsp"
        };
        Tree.Add("category_comp", list);

        list = new List<string>
        {
            "ref_encyc",
            "ref_dict",
            "ref_ref",
            "ref_guide",
            "reference",
            "design",
            "geo_guides"
        };
        Tree.Add("category_ref", list);

        list = new List<string>
        {
            "religion_rel",
            "religion_esoterics",
            "religion_self",
            "religion",
            "religion_budda",
            "religion_christianity",
            "religion_orthodoxy",
            "religion_catholicism",
            "religion_protestantism",
            "religion_hinduism",
            "religion_islam",
            "religion_judaism",
            "astrology",
            "palmistry",
            "religion_paganism"
        };
        Tree.Add("category_religion", list);

        list = new List<string>
        {
            "sf_humor",
            "humor_anecdote",
            "humor_prose",
            "humor_verse",
            "humor",
            "comedy",
            "humor_satire"
        };
        Tree.Add("category_humor", list);

        list = new List<string>
        {
            "home_cooking",
            "home_pets",
            "home_crafts",
            "home_entertain",
            "home_health",
            "home_garden",
            "home_diy",
            "home_sport",
            "home_sex",
            "home",
            "home_collecting"
        };
        Tree.Add("category_home", list);

        list = new List<string>
        {
            "sci_tech",
            "sci_transport",
            "sci_metal",
            "sci_radio",
            "sci_build",
            "auto_regulations",
            "architecture_book"
        };
        Tree.Add("category_technics", list);

        list = new List<string>
        {
            "other",
            "notes",
            "periodic",
            "music",
            "cine",
            "theatre",
            "fanfiction",
            "unfinished",
            "visual_arts"
        };
        Tree.Add("category_other", list);

        list = new List<string>
        {
            "banking",
            "accounting",
            "global_economy",
            "paper_work",
            "org_behavior",
            "personal_finance",
            "small_business",
            "marketing",
            "real_estate",
            "popular_business",
            "industries",
            "job_hunting",
            "ya",
            "management",
            "stock",
            "economics",
            "trade"
        };
        Tree.Add("category_business", list);

        list = new List<string>
        {
            "adv_geo",
            "adv_animal",
            "nonf_biography",
            "nonf_publicism",
            "nonf_criticism",
            "nonfiction",
            "nonf_military",
            "sci_popular"
        };
        Tree.Add("category_nonfiction", list);

        list = new List<string>
        {
            "dramaturgy",
            "drama",
            "screenplays",
            "comedy",
            "mystery",
            "scenarios",
            "tragedy",
            "vaudeville"
        };
        Tree.Add("category_drama", list);

        list = new List<string>
        {
            "humor_anecdote",
            "epic",
            "child_folklore",
            "riddles",
            "folk_songs",
            "folk_tale",
            "proverbs",
            "folklore",
            "limerick"
        };
        Tree.Add("category_folk", list);

        list = new List<string>
        {
            "prose_military",
            "nonf_military",
            "military_history",
            "military_weapon",
            "military_arts",
            "military_special",
            "military"
        };
        Tree.Add("category_military", list);
    }
}
