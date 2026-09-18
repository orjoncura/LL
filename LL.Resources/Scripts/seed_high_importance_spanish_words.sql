-- Seed the top 100 Spanish words with ImportanceRating = High (Id = 1).
-- Also creates English WordLinks (LanguageId = 1) plus a basic meaning/definition
-- so GetKeyWords and flashcards can resolve them.
--
-- LanguageId: Spanish = 2, English = 1
-- ImportanceRatingId: High = 1
-- CreatedById: Admin = -1
-- ContentEntries: empty audio placeholder (regenerate TTS later if needed)
--
-- Safe to re-run: existing words are upgraded to High; missing links/meanings are added.

BEGIN;

DO $$
DECLARE
  r RECORD;
  v_word_id integer;
  v_doc_id integer;
  v_meaning_id integer;
  v_user_id integer := -1;
  v_spanish integer := 2;
  v_english integer := 1;
  v_high integer := 1;
BEGIN
  FOR r IN
    SELECT * FROM (VALUES
      ('que',    'that / which',                          7, 'that / which'),
      ('de',     'of / from',                             6, 'of / from'),
      ('no',     'no / not',                              4, 'no / not'),
      ('a',      'to / at',                               6, 'to / at'),
      ('la',     'the (feminine singular)',               8, 'the (feminine singular)'),
      ('el',     'the (masculine singular)',              8, 'the (masculine singular)'),
      ('es',     'is (permanent/essential)',              2, 'is (permanent/essential)'),
      ('y',      'and',                                   4, 'and'),
      ('en',     'in / on / at',                          6, 'in / on / at'),
      ('lo',     'it / him (direct object)',              7, 'it / him (direct object)'),
      ('un',     'a / an (masculine singular)',           8, 'a / an (masculine singular)'),
      ('por',    'for / by / through',                    6, 'for / by / through'),
      ('qué',    'what? / how?',                          7, 'what? / how?'),
      ('me',     'me / myself',                           7, 'me / myself'),
      ('una',    'a / an (feminine singular)',            8, 'a / an (feminine singular)'),
      ('te',     'you (informal direct object)',          7, 'you (informal direct object)'),
      ('los',    'the (masculine plural)',                8, 'the (masculine plural)'),
      ('se',     'himself / herself / itself / yourself', 7, 'himself / herself / itself / yourself'),
      ('con',    'with',                                  6, 'with'),
      ('para',   'for / to / in order to',                6, 'for / to / in order to'),
      ('mi',     'my',                                    8, 'my'),
      ('está',   'is (temporary/location)',               2, 'is (temporary/location)'),
      ('si',     'if',                                    4, 'if'),
      ('bien',   'well / good',                           4, 'well / good'),
      ('pero',   'but',                                   4, 'but'),
      ('yo',     'I',                                     7, 'I'),
      ('eso',    'that (pronoun)',                        7, 'that (pronoun)'),
      ('las',    'the (feminine plural)',                 8, 'the (feminine plural)'),
      ('sí',     'yes',                                   5, 'yes'),
      ('su',     'his / her / your (formal) / their',     8, 'his / her / your (formal) / their'),
      ('tu',     'your (informal)',                       8, 'your (informal)'),
      ('aquí',   'here',                                  4, 'here'),
      ('del',    'of the / from the',                     6, 'of the / from the'),
      ('al',     'to the',                                6, 'to the'),
      ('como',   'how / as / like',                       4, 'how / as / like'),
      ('le',     'him / her / you (formal indirect object)', 7, 'him / her / you (formal indirect object)'),
      ('más',    'more',                                  4, 'more'),
      ('esto',   'this (masculine singular)',             7, 'this (masculine singular)'),
      ('ya',     'already / now',                         4, 'already / now'),
      ('todo',   'all / everything',                      7, 'all / everything'),
      ('esta',   'this (feminine singular)',              8, 'this (feminine singular)'),
      ('vamos',  'let''s go',                             2, 'let''s go'),
      ('muy',    'very',                                  4, 'very'),
      ('hay',    'there is / there are',                  2, 'there is / there are'),
      ('ahora',  'now',                                   4, 'now'),
      ('algo',   'something',                             7, 'something'),
      ('estoy',  'I am (temporary/location)',             2, 'I am (temporary/location)'),
      ('tengo',  'I have',                                2, 'I have'),
      ('nos',    'us / ourselves',                        7, 'us / ourselves'),
      ('tú',     'you (informal subject)',                7, 'you (informal subject)'),
      ('nada',   'nothing',                               7, 'nothing'),
      ('cuando', 'when',                                  4, 'when'),
      ('ha',     'has (auxiliary verb)',                  2, 'has (auxiliary verb)'),
      ('este',   'this (masculine singular)',             8, 'this (masculine singular)'),
      ('sé',     'I know',                                2, 'I know'),
      ('estás',  'you are (temporary/location)',          2, 'you are (temporary/location)'),
      ('así',    'like this / so',                        4, 'like this / so'),
      ('puedo',  'I can',                                 2, 'I can'),
      ('cómo',   'how?',                                  4, 'how?'),
      ('quiero', 'I want',                                2, 'I want'),
      ('sólo',   'only / just',                           4, 'only / just'),
      ('solo',   'only / just',                           4, 'only / just'),
      ('soy',    'I am (permanent/essential)',            2, 'I am (permanent/essential)'),
      ('tiene',  'he/she has / you have',                 2, 'he/she has / you have'),
      ('gracias','thank you',                             5, 'thank you'),
      ('o',      'or',                                    4, 'or'),
      ('él',     'he',                                    7, 'he'),
      ('bueno',  'good',                                  3, 'good'),
      ('fue',    'he/she/it was / went',                  2, 'he/she/it was / went'),
      ('ser',    'to be (permanent)',                     2, 'to be (permanent)'),
      ('hacer',  'to do / to make',                       2, 'to do / to make'),
      ('son',    'they are (permanent)',                  2, 'they are (permanent)'),
      ('todos',  'all / everyone',                        7, 'all / everyone'),
      ('era',    'was (imperfect tense)',                 2, 'was (imperfect tense)'),
      ('eres',   'you are (permanent)',                   2, 'you are (permanent)'),
      ('vez',    'time (occurrence)',                     1, 'time (occurrence, e.g. one time)'),
      ('tienes', 'you have',                              2, 'you have'),
      ('creo',   'I believe / I think',                   2, 'I believe / I think'),
      ('ella',   'she',                                   7, 'she'),
      ('he',     'I have (auxiliary verb)',               2, 'I have (auxiliary verb)'),
      ('ese',    'that (masculine)',                      8, 'that (masculine)'),
      ('voy',    'I go / I''m going',                     2, 'I go / I''m going'),
      ('puede',  'he/she can / you can',                  2, 'he/she can / you can'),
      ('sabes',  'you know',                              2, 'you know'),
      ('hola',   'hello',                                 5, 'hello'),
      ('sus',    'his / her / your / their (plural)',     8, 'his / her / your / their (plural)'),
      ('porque', 'because',                               4, 'because'),
      ('dios',   'God',                                   1, 'God'),
      ('quién',  'who?',                                  7, 'who?'),
      ('nunca',  'never',                                 4, 'never'),
      ('dónde',  'where?',                                4, 'where?'),
      ('quieres','you want',                              2, 'you want'),
      ('casa',   'house / home',                          1, 'house / home'),
      ('favor',  'favor',                                 1, 'favor'),
      ('esa',    'that (feminine)',                       8, 'that (feminine)'),
      ('dos',    'two',                                   8, 'two'),
      ('tan',    'so / as',                               4, 'so / as'),
      ('señor',  'mister / sir',                          1, 'mister / sir'),
      ('tiempo', 'time / weather',                        1, 'time / weather'),
      ('verdad', 'truth / right?',                        1, 'truth / right?'),
      ('estaba', 'I was / he/she was (imperfect)',        2, 'I was / he/she was (imperfect)')
    ) AS t(name, translation, type_id, definition)
  LOOP
    SELECT w."Id"
    INTO v_word_id
    FROM "Words" w
    WHERE lower(w."Name") = lower(r.name)
      AND w."LanguageId" = v_spanish
    LIMIT 1;

    IF v_word_id IS NULL THEN
      INSERT INTO "ContentEntries" ("Content")
      VALUES ('\x'::bytea)
      RETURNING "Id" INTO v_doc_id;

      INSERT INTO "Words" (
        "Name",
        "DocumentId",
        "LanguageId",
        "ImportanceRatingId",
        "IsActive",
        "CreatedById",
        "CreatedDate"
      )
      VALUES (
        lower(r.name),
        v_doc_id,
        v_spanish,
        v_high,
        true,
        v_user_id,
        NOW()
      )
      RETURNING "Id" INTO v_word_id;
    ELSE
      UPDATE "Words"
      SET "ImportanceRatingId" = v_high
      WHERE "Id" = v_word_id
        AND "ImportanceRatingId" <> v_high;
    END IF;

    IF NOT EXISTS (
      SELECT 1
      FROM "WordLinks" wl
      WHERE wl."WordId" = v_word_id
        AND wl."LanguageId" = v_english
        AND wl."IsActive" = true
    ) THEN
      INSERT INTO "WordLinks" (
        "WordId",
        "Value",
        "LanguageId",
        "IsActive",
        "CreatedById",
        "CreatedDate"
      )
      VALUES (
        v_word_id,
        r.translation,
        v_english,
        true,
        v_user_id,
        NOW()
      );
    END IF;

    SELECT wm."Id"
    INTO v_meaning_id
    FROM "WordMeanings" wm
    WHERE wm."WordId" = v_word_id
      AND wm."IsActive" = true
    LIMIT 1;

    IF v_meaning_id IS NULL THEN
      INSERT INTO "WordMeanings" (
        "WordId",
        "TypeId",
        "IsActive",
        "CreatedById",
        "CreatedDate"
      )
      VALUES (
        v_word_id,
        r.type_id,
        true,
        v_user_id,
        NOW()
      )
      RETURNING "Id" INTO v_meaning_id;

      INSERT INTO "WordDefinitions" (
        "Value",
        "WordMeaningId",
        "IsActive",
        "CreatedById",
        "CreatedDate"
      )
      VALUES (
        r.definition,
        v_meaning_id,
        true,
        v_user_id,
        NOW()
      );
    END IF;
  END LOOP;
END $$;

COMMIT;
