import { OperationParameter, OperationURLParameter } from "@azure/core-client";
import {
  LoginModel as LoginModelMapper,
  SeminarRequestModel as SeminarRequestModelMapper,
} from "../models/mappers";

export const contentType: OperationParameter = {
  parameterPath: ["options", "contentType"],
  mapper: {
    defaultValue: "application/json",
    isConstant: true,
    serializedName: "Content-Type",
    type: {
      name: "String",
    },
  },
};

export const body: OperationParameter = {
  parameterPath: ["options", "body"],
  mapper: LoginModelMapper,
};

export const accept: OperationParameter = {
  parameterPath: "accept",
  mapper: {
    defaultValue: "application/json, text/json",
    isConstant: true,
    serializedName: "Accept",
    type: {
      name: "String",
    },
  },
};

export const $host: OperationURLParameter = {
  parameterPath: "$host",
  mapper: {
    serializedName: "$host",
    required: true,
    type: {
      name: "String",
    },
  },
  skipEncoding: true,
};

export const body1: OperationParameter = {
  parameterPath: ["options", "body"],
  mapper: SeminarRequestModelMapper,
};
