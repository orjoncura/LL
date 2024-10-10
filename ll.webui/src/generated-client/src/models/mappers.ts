import * as coreClient from "@azure/core-client";

export const LoginModel: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "LoginModel",
    modelProperties: {
      email: {
        serializedName: "email",
        nullable: true,
        type: {
          name: "String",
        },
      },
      password: {
        serializedName: "password",
        nullable: true,
        type: {
          name: "String",
        },
      },
      isValid: {
        serializedName: "isValid",
        readOnly: true,
        type: {
          name: "Boolean",
        },
      },
    },
  },
};

export const TokenViewModel: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "TokenViewModel",
    modelProperties: {
      token: {
        serializedName: "token",
        nullable: true,
        type: {
          name: "String",
        },
      },
      expiration: {
        serializedName: "expiration",
        nullable: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const ProblemDetails: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "ProblemDetails",
    additionalProperties: { type: { name: "Object" } },
    modelProperties: {
      type: {
        serializedName: "type",
        nullable: true,
        type: {
          name: "String",
        },
      },
      title: {
        serializedName: "title",
        nullable: true,
        type: {
          name: "String",
        },
      },
      status: {
        serializedName: "status",
        nullable: true,
        type: {
          name: "Number",
        },
      },
      detail: {
        serializedName: "detail",
        nullable: true,
        type: {
          name: "String",
        },
      },
      instance: {
        serializedName: "instance",
        nullable: true,
        type: {
          name: "String",
        },
      },
    },
  },
};

export const SeminarRequestModel: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "SeminarRequestModel",
    modelProperties: {
      text: {
        serializedName: "text",
        nullable: true,
        type: {
          name: "String",
        },
      },
      languageFromId: {
        serializedName: "languageFromId",
        type: {
          name: "Number",
        },
      },
      languageToId: {
        serializedName: "languageToId",
        type: {
          name: "Number",
        },
      },
      isValid: {
        serializedName: "isValid",
        readOnly: true,
        type: {
          name: "Boolean",
        },
      },
    },
  },
};

export const SeminarViewModel: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "SeminarViewModel",
    modelProperties: {
      targetWord: {
        serializedName: "targetWord",
        nullable: true,
        type: {
          name: "String",
        },
      },
      sentences: {
        serializedName: "sentences",
        nullable: true,
        type: {
          name: "Sequence",
          element: {
            type: {
              name: "Composite",
              className: "StatementShort",
            },
          },
        },
      },
      importance: {
        serializedName: "importance",
        type: {
          name: "Number",
        },
      },
      isValid: {
        serializedName: "isValid",
        readOnly: true,
        type: {
          name: "Boolean",
        },
      },
    },
  },
};

export const StatementShort: coreClient.CompositeMapper = {
  type: {
    name: "Composite",
    className: "StatementShort",
    modelProperties: {
      originalStatement: {
        serializedName: "originalStatement",
        nullable: true,
        type: {
          name: "String",
        },
      },
      translatedStatement: {
        serializedName: "translatedStatement",
        nullable: true,
        type: {
          name: "String",
        },
      },
      isValid: {
        serializedName: "isValid",
        readOnly: true,
        type: {
          name: "Boolean",
        },
      },
    },
  },
};
