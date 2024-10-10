import * as coreClient from "@azure/core-client";
import * as coreRestPipeline from "@azure/core-rest-pipeline";
import * as coreAuth from "@azure/core-auth";
import * as Parameters from "./models/parameters";
import * as Mappers from "./models/mappers";
import {
  ApiOptionalParams,
  ApiSecurityOptionalParams,
  ApiSecurityResponse,
  ApiSeminarOptionalParams,
  ApiSeminarResponse,
} from "./models";

export class Api extends coreClient.ServiceClient {
  $host: string;

  /**
   * Initializes a new instance of the Api class.
   * @param credentials Subscription credentials which uniquely identify client subscription.
   * @param $host server parameter
   * @param options The parameter options
   */
  constructor(
    credentials: coreAuth.TokenCredential,
    $host: string,
    options?: ApiOptionalParams,
  ) {
    if (credentials === undefined) {
      throw new Error("'credentials' cannot be null");
    }
    if ($host === undefined) {
      throw new Error("'$host' cannot be null");
    }

    // Initializing default values for options
    if (!options) {
      options = {};
    }
    const defaults: ApiOptionalParams = {
      requestContentType: "application/json; charset=utf-8",
      credential: credentials,
    };

    const packageDetails = `azsdk-js-api/1.0.0-beta.1`;
    const userAgentPrefix =
      options.userAgentOptions && options.userAgentOptions.userAgentPrefix
        ? `${options.userAgentOptions.userAgentPrefix} ${packageDetails}`
        : `${packageDetails}`;

    if (!options.credentialScopes) {
      options.credentialScopes = ["https://management.azure.com/.default"];
    }
    const optionsWithDefaults = {
      ...defaults,
      ...options,
      userAgentOptions: {
        userAgentPrefix,
      },
      endpoint: options.endpoint ?? options.baseUri ?? "{$host}",
    };
    super(optionsWithDefaults);

    let bearerTokenAuthenticationPolicyFound: boolean = false;
    if (options?.pipeline && options.pipeline.getOrderedPolicies().length > 0) {
      const pipelinePolicies: coreRestPipeline.PipelinePolicy[] =
        options.pipeline.getOrderedPolicies();
      bearerTokenAuthenticationPolicyFound = pipelinePolicies.some(
        (pipelinePolicy) =>
          pipelinePolicy.name ===
          coreRestPipeline.bearerTokenAuthenticationPolicyName,
      );
    }
    if (
      !options ||
      !options.pipeline ||
      options.pipeline.getOrderedPolicies().length == 0 ||
      !bearerTokenAuthenticationPolicyFound
    ) {
      this.pipeline.removePolicy({
        name: coreRestPipeline.bearerTokenAuthenticationPolicyName,
      });
      this.pipeline.addPolicy(
        coreRestPipeline.bearerTokenAuthenticationPolicy({
          credential: credentials,
          scopes:
            optionsWithDefaults.credentialScopes ??
            `${optionsWithDefaults.endpoint}/.default`,
          challengeCallbacks: {
            authorizeRequestOnChallenge:
              coreClient.authorizeRequestOnClaimChallenge,
          },
        }),
      );
    }
    // Parameter assignments
    this.$host = $host;
  }

  /** @param options The options parameters. */
  security(options?: ApiSecurityOptionalParams): Promise<ApiSecurityResponse> {
    return this.sendOperationRequest({ options }, securityOperationSpec);
  }

  /** @param options The options parameters. */
  seminar(options?: ApiSeminarOptionalParams): Promise<ApiSeminarResponse> {
    return this.sendOperationRequest({ options }, seminarOperationSpec);
  }
}
// Operation Specifications
const serializer = coreClient.createSerializer(Mappers, /* isXml */ false);

const securityOperationSpec: coreClient.OperationSpec = {
  path: "/Security/Authenticate",
  httpMethod: "POST",
  responses: {
    200: {
      bodyMapper: {
        type: {
          name: "Sequence",
          element: { type: { name: "Composite", className: "TokenViewModel" } },
        },
      },
    },
    401: {
      bodyMapper: Mappers.ProblemDetails,
    },
    500: {},
  },
  requestBody: Parameters.body,
  urlParameters: [Parameters.$host],
  headerParameters: [Parameters.contentType, Parameters.accept],
  mediaType: "json",
  serializer,
};
const seminarOperationSpec: coreClient.OperationSpec = {
  path: "/Seminar/CreateSeminar",
  httpMethod: "POST",
  responses: {
    200: {
      bodyMapper: {
        type: {
          name: "Sequence",
          element: {
            type: { name: "Composite", className: "SeminarViewModel" },
          },
        },
      },
    },
    401: {
      bodyMapper: Mappers.ProblemDetails,
    },
    500: {},
  },
  requestBody: Parameters.body1,
  urlParameters: [Parameters.$host],
  headerParameters: [Parameters.contentType, Parameters.accept],
  mediaType: "json",
  serializer,
};
